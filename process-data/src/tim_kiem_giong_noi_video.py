import subprocess
import os
import librosa
import numpy as np
from scipy.spatial.distance import cdist
from scipy.signal import butter, lfilter

def extract_audio(input_video):
    # Tạo đường dẫn tệp đầu ra với phần mở rộng .wav
    output_audio = os.path.splitext(input_video)[0] + '.wav'
    
    # Lệnh ffmpeg để trích xuất âm thanh từ video và lưu dưới dạng WAV
    command = [
        'ffmpeg',
        '-y',  # Thêm tùy chọn này để tự động ghi đè tệp đầu ra nếu đã tồn tại
        '-i', input_video,
        '-vn',  # Bỏ phần video
        '-acodec', 'pcm_s16le',  # Sử dụng codec PCM 16-bit little-endian
        '-ar', '44100',  # Tần số mẫu (sample rate)
        '-ac', '2',  # Số lượng kênh (2 kênh - stereo)
        output_audio
    ]
    
    # Chạy lệnh ffmpeg
    subprocess.run(command, check=True)
    return f"{output_audio}"

# Hàm để áp dụng lọc băng thông (bandpass filter) để loại bỏ tiếng ồn
def bandpass_filter(data, lowcut, highcut, fs, order=5):
    nyquist = 0.5 * fs
    low = lowcut / nyquist
    high = highcut / nyquist
    b, a = butter(order, [low, high], btype='band')
    y = lfilter(b, a, data)
    return y

# Hàm để trích xuất đặc trưng MFCC từ âm thanh với tiền xử lý
def extract_features(audio_file):
    y, sr = librosa.load(audio_file, sr=None)
    
    # Áp dụng lọc băng thông
    y = bandpass_filter(y, 300, 3400, sr)  # Giới hạn tần số từ 300Hz đến 3400Hz
    
    # Chuẩn hóa âm lượng
    y = librosa.util.normalize(y)
    
    mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=13)
    return mfcc.T  # Trả về đặc trưng dạng ma trận, mỗi dòng là một vector đặc trưng

# Hàm để so sánh đặc trưng giữa hai âm thanh
def compare_features(sample_features, test_features):
    # Tính khoảng cách Euclidean giữa mỗi đặc trưng của đoạn kiểm tra với tất cả đặc trưng mẫu
    distances = cdist(test_features, sample_features, metric='euclidean')
    min_distance = np.min(distances, axis=1)  # Lấy khoảng cách tối thiểu cho mỗi đoạn
    return np.mean(min_distance)  # Trả về khoảng cách trung bình

def merge_time_segments(segments):
    merged_segments = []
    current_start = None
    current_end = None
    
    for segment in segments:
        start, end = map(float, segment.split(' ')[4::2])  # Chuyển đổi thành số thập phân
        
        if current_start is None:
            current_start = start
            current_end = end
        elif start <= current_end + 3:
            current_end = end
        else:
            merged_segments.append((current_start, current_end))
            current_start = start
            current_end = end
            
    if current_start is not None and current_end is not None:
        merged_segments.append((current_start, current_end))
    
    return merged_segments

def process(sample_audio_file_input, test_video_file_inputs):
    # Trích xuất đặc trưng từ âm thanh mẫu
    sample_features = extract_features(sample_audio_file_input)
    if sample_features is None:
        return ["Không thể truy cập tệp âm thanh mẫu"]
    
    results = []
    for test_video_file_input in test_video_file_inputs:
        input_video = test_video_file_input
        extracted_audio = extract_audio(input_video)
        print(f"test_video_file_input: {test_video_file_input}")
        test_audio_file = extracted_audio
        try:
            y, sr = librosa.load(test_audio_file, sr=None)
        except PermissionError as e:
            print(f"Permission denied: {e}")
            results.append(f"Không thể truy cập tệp âm thanh kiểm tra: {test_audio_file}")
            continue

        # Áp dụng lọc băng thông cho toàn bộ âm thanh
        y = bandpass_filter(y, 300, 3400, sr)
        y = librosa.util.normalize(y)

        duration = librosa.get_duration(y=y, sr=sr)
        segment_length = 1  # Độ dài của mỗi đoạn là 1 giây
        start_time = 0

        # Duyệt qua các đoạn âm thanh trong file cần kiểm tra
        output = []
        while start_time < duration:
            end_time = min(start_time + segment_length, duration)
            segment = y[int(start_time * sr):int(end_time * sr)]
            segment_features = librosa.feature.mfcc(y=segment, sr=sr, n_mfcc=13).T

            # So sánh đặc trưng của đoạn âm thanh với đặc trưng mẫu
            distance = compare_features(sample_features, segment_features)
            # print(f"Average distance for segment {start_time}-{end_time}s: {distance}")

            # Kiểm tra nếu khoảng cách nhỏ hơn một ngưỡng thì coi như đã tìm thấy giọng nói mẫu
            if distance < 40:  # Thay đổi ngưỡng theo nhu cầu
                # print(f"Sample voice detected between {start_time} and {end_time} seconds")
                output.append(f"Sample voice detected between {start_time} and {end_time} seconds")
            
            start_time += segment_length
            
            
        merged_segments = merge_time_segments(output)
        output_text = ""
        count = 0
        for start, end in merged_segments:
            print(f"Sample voice detected between {start} and {end} seconds")
            output_text += f"Phát hiện giọng nói từ {start} đến {end} giây\n"
            if(end > start + 2):
                count+=1
        
        if output_text == "" or count < 2:
            results.append(False)
        else:
            results.append(output_text)
    
    return results

def tim_kiem_giong_noi_video(sample_audio_file_input, test_video_file_inputs):
    return process(sample_audio_file_input, test_video_file_inputs)

# print(tim_kiem_giong_noi_video(r'E:\Do An\process-data\output_audio\segment_10.wav', [r'E:\Do An\process-data\data\1.mp4']))
