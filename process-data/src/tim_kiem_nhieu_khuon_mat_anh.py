import face_recognition

def tim_kiem_nhieu_khuon_mat_anh(image_Path, image_Path2, paths):
    results = []

    known_image = face_recognition.load_image_file(image_Path)
    known_encodings = face_recognition.face_encodings(known_image)

    if not known_encodings:
        raise ValueError("No faces found in the first known image.")
    
    known_image2 = face_recognition.load_image_file(image_Path2)
    known_encodings2 = face_recognition.face_encodings(known_image2)

    if not known_encodings2:
        raise ValueError("No faces found in the second known image.")

    for path in paths:
        unknown_image = face_recognition.load_image_file(path)
        unknown_encodings = face_recognition.face_encodings(unknown_image)
        
        if unknown_encodings:
            match_found = False
            for unknown_encoding in unknown_encodings:
                check = face_recognition.compare_faces(known_encodings, unknown_encoding, tolerance=0.5)
                check2 = face_recognition.compare_faces(known_encodings2, unknown_encoding, tolerance=0.5)
                
                if any(check) and any(check2):
                    match_found = True
                    break
            
            results.append(match_found)
        else:
            results.append(False) 
    
    return results
