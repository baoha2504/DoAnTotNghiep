from docx import Document

def read_file_word(path):
    try:
        doc = Document(path)
        text = []
        for para in doc.paragraphs:
            text.append(para.text)
        return '\n'.join(text)
    except Exception as e:
        print(f"An error occurred: {e}")
        return ''
