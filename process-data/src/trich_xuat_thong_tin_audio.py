import whisper
from phan_tich_cam_xuc import detect_sentiment

def audio_to_text(audioPath):
    model = whisper.load_model("base")
    result = model.transcribe(audioPath)
    if result["text"].strip() == "":
        return "Không có âm thanh trong tệp"
    else:
        return result["text"]

def trich_xuat_thong_tin_audio(audioPath):
    text = audio_to_text(audioPath)
    if text == "Không có âm thanh trong tệp":
        sentiment = "Không có âm thanh trong tệp"
    else:
        sentiment = detect_sentiment(text[:500])
    return {"path": audioPath, "sentiment": sentiment, "text": text}
        