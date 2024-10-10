import subprocess
import os

def video_to_wav(input_video):
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

# Đường dẫn tới tệp MP4 và tệp WAV đầu ra
input_video = r"E:\Do An\process-data\1.mp4"

print(video_to_wav(input_video))
