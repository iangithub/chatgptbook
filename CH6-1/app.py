import whisper
tiny_model = whisper.load_model("tiny")
result = tiny_model.transcribe("intro.m4a")
print(result)

