import whisper

def audio_to_text(audioPath):
    model = whisper.load_model("base")
    result = model.transcribe(audioPath)
    
    if result["text"].strip() == "":
        return "Không có âm thanh trong tệp"
    else:
        return result["text"]

print(audio_to_text(r"E:\Do An\process-data\data\2.wav"))