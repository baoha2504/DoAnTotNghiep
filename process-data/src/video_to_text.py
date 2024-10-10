import whisper
from moviepy.editor import VideoFileClip
import os

def video_to_text(videoPath):
    # Load video file
    video = VideoFileClip(videoPath)

    # Extract audio from video
    audio = video.audio

    # Export audio to a temporary file (whisper supports only WAV or MP3)
    temp_audio_path = "temp_audio.wav"
    audio.write_audiofile(temp_audio_path)

    # Load the model
    model = whisper.load_model("base")

    # Transcribe audio
    result = model.transcribe(temp_audio_path)

    os.remove(temp_audio_path)
    return result["text"]
    
videoPath = r"E:\Do An\process-data\data\2.mp4"
print(video_to_text(videoPath))

