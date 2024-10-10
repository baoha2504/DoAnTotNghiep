import whisper
import subprocess
import os
from phan_tich_cam_xuc import detect_sentiment

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


def audio_to_text(audioPath):
    model = whisper.load_model("base")
    result = model.transcribe(audioPath)
    if result["text"].strip() == "":
        return "Không có âm thanh trong tệp"
    else:
        return result["text"]

def trich_xuat_thong_tin_audio_trong_video(videoPath):
    audioPath = video_to_wav(videoPath)
    text = audio_to_text(audioPath)
    if text == "Không có âm thanh trong tệp":
        sentiment = "Không có âm thanh trong tệp"
    else:
        sentiment = detect_sentiment(text[:500])
    return {"path": videoPath, "sentiment": sentiment, "text": text}
