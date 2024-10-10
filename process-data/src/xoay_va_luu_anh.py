from PIL import Image
import shutil
import os

def move_and_rotate_image(image_path, target_folder, rotation_degrees):
    try:
        # Kiểm tra nếu file tồn tại
        if not os.path.isfile(image_path):
            print(f"File not found: {image_path}")
            return
        
        # Kiểm tra nếu folder tồn tại, nếu không thì tạo mới
        if not os.path.exists(target_folder):
            os.makedirs(target_folder)
        
        # Mở ảnh
        with Image.open(image_path) as img:
            # Xoay ảnh và lấp đầy các vị trí trống bằng màu trắng
            rotated_img = img.rotate(-rotation_degrees, expand=True, fillcolor='white')
            
            # Lấy tên file từ path đầu vào
            file_name = os.path.basename(image_path)
            
            # Đường dẫn file mới trong folder đích
            target_path = os.path.join(target_folder, file_name)
            
            # Lưu ảnh đã xoay vào folder đích
            rotated_img.save(target_path)
            
            print(f"Image saved to: {target_path}")
            return "success"
    
    except Exception as e:
        print(f"An error occurred: {e}")
        return "error"


def xoay_va_luu_anh(image_path, target_folder, rotation_degrees_str):
    try:
        rotation_degrees = float(rotation_degrees_str)
    except ValueError:
        print("Lỗi: Giá trị 'rotation_degrees' không hợp lệ.")
        return
    
    return move_and_rotate_image(image_path, target_folder, rotation_degrees)
