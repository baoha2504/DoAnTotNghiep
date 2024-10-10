import fitz

def read_file_pdf(path):
    try:
        text = []
        pdf_document = fitz.open(path)
        for page_num in range(len(pdf_document)):
            page = pdf_document.load_page(page_num)
            text.append(page.get_text())
        pdf_document.close()
        return '\n'.join(text)
    except Exception as e:
        print(f"An error occurred: {e}")
        return ''
