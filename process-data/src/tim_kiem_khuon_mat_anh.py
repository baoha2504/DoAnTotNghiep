import face_recognition

def tim_kiem_khuon_mat_anh(image_Path, paths):
    results = []

    # Tải hình ảnh đã biết và mã hóa khuôn mặt
    known_image = face_recognition.load_image_file(image_Path)
    known_encodings = face_recognition.face_encodings(known_image)

    if not known_encodings:
        raise ValueError("No faces found in the known image.")
    
    known_encoding = known_encodings[0]

    # Duyệt qua các đường dẫn hình ảnh chưa biết
    for path in paths:
        unknown_image = face_recognition.load_image_file(path)
        unknown_encodings = face_recognition.face_encodings(unknown_image)
        
        if unknown_encodings:
            match_found = False
            for unknown_encoding in unknown_encodings:
                check = face_recognition.compare_faces([known_encoding], unknown_encoding, tolerance=0.5)
                # print(face_recognition.compare_faces([known_encoding], unknown_encoding))
                if any(check):
                    match_found = True
                    break
            
            results.append(match_found)
        else:
            results.append(False) 

    return results

# # Example usage
# known_image_path = r"E:\Do An\process-data\data\QuocBao.jpg"
# unknown_image_paths = [
#     r"E:\Do An\process-data\data\anh.jpg",
#     r"C:\Users\hacon\Desktop\anh.jpg",
#     r"C:\Users\hacon\Desktop\anh.png",
#     r"E:\Do An\process-data\data\DongThuong.png",
#     r"C:\Users\hacon\Desktop\data_test\6.jpg"
# ]

# # Call the function and get the results
# results = tim_kiem_khuon_mat_anh(known_image_path, unknown_image_paths)

# print(results)