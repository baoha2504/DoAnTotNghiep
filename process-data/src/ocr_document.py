from PIL import Image
import pytesseract
import cv2
import os

def recognize_text(image_path, output_file, preprocess='thresh'):
    # Đọc file ảnh và chuyển về ảnh xám
    image = cv2.imread(image_path)
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    
    # Check xem có sử dụng tiền xử lý ảnh không
    # Nếu phân tách đen trắng
    if preprocess == 'thresh':
        gray = cv2.threshold(gray, 0, 255,
            cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]
    
    # Nếu làm mờ ảnh
    elif preprocess == 'blur':
        gray = cv2.medianBlur(gray, 3)
    
    # Ghi ảnh đã xử lý xuống ổ cứng
    cv2.imwrite(output_file, gray)

    # Load ảnh và apply nhận dạng bằng Tesseract OCR
    text = pytesseract.image_to_string(Image.open(output_file), lang='vie')
    
    # Trả về đường dẫn của ảnh đã xử lý
    return text, output_file

def ocr_document(filePath, outputPath):
    text, filename = recognize_text(filePath, outputPath)
    return {"text": text, "filename": filename}
