import face_recognition
from PIL import Image
import os

def trich_xuat_khuon_mat_anh(imagePath):
    results = []
    
    image = face_recognition.load_image_file(imagePath)
    # Detect faces
    face_locations = face_recognition.face_locations(image)
    
    # Get the base name of the image file
    base_name = os.path.basename(imagePath)
    # Remove file extension
    base_name_without_ext = os.path.splitext(base_name)[0]
    
    # Create directory with the same name as the image file
    directory_path = os.path.join(os.path.dirname(imagePath), base_name_without_ext)
    os.makedirs(directory_path, exist_ok=True)

    for i, face_location in enumerate(face_locations):
        top, right, bottom, left = face_location
        # Extract face
        face_image = image[top:bottom, left:right]
        # Convert to PIL image
        pil_image = Image.fromarray(face_image)
        # Save image with the specified naming convention
        face_image_path = os.path.join(directory_path, f"{base_name_without_ext}_face_{i+1}.jpg")
        results.append(face_image_path)
        
        pil_image.save(face_image_path)
    
    return results

# # Path to the input image
# image_path = r"C:\Users\hacon\Desktop\3.jpg"

# # Extract faces and save them
# print(trich_xuat_khuon_mat_anh(image_path))
