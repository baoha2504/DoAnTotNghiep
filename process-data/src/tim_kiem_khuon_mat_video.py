import face_recognition
import cv2
import numpy as np

def process(pathImage, pathVideo):
    video_capture = cv2.VideoCapture(pathVideo)

    object_image = face_recognition.load_image_file(pathImage)
    object_face_encoding = face_recognition.face_encodings(object_image)[0]

    known_face_encodings = [
        object_face_encoding
    ]

    known_face_names = [
        "Đối tượng"
    ]

    # Initialize some variables
    face_locations = []
    face_encodings = []
    face_names = []
    frame_counter = 0
    fps = 3  # Process n frames per second
    process_this_frame = True
    result = ''

    object_found = False
    start_time = None

    while True:
        # Grab a single frame of video
        ret, frame = video_capture.read()
        
        # Check if the video has ended
        if not ret:
            print("End of video")
            break

        # Get the current time of the video in milliseconds
        current_time_ms = video_capture.get(cv2.CAP_PROP_POS_MSEC)
        current_time_sec = current_time_ms / 1000  # Convert to seconds

        # Process only n frames per second (fps)
        frame_counter += 1
        if frame_counter % int(video_capture.get(cv2.CAP_PROP_FPS) // fps) != 0:
            continue

        # Resize frame of video to 1/4 size for faster face recognition processing
        small_frame = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)

        # Convert the image from BGR color (which OpenCV uses) to RGB color (which face_recognition uses)
        rgb_small_frame = small_frame[:, :, ::-1]

        # Find all the faces and face encodings in the current frame of video
        face_locations = face_recognition.face_locations(rgb_small_frame)
        face_encodings = face_recognition.face_encodings(rgb_small_frame, face_locations)

        face_names = []
        object_currently_in_frame = False  # Track if object is in this frame

        for face_encoding in face_encodings:
            # Calculate face distances
            face_distances = face_recognition.face_distance(known_face_encodings, face_encoding)

            best_match_index = np.argmin(face_distances)
            if face_distances[best_match_index] < 0.5:
                name = known_face_names[best_match_index]
            else:
                name = "Unknown"

            face_names.append(name)

            # Check if object is detected
            if name != "Unknown":
                object_currently_in_frame = True
                minutes = int(current_time_sec // 60)
                seconds = int(current_time_sec % 60)
                
                if not object_found:
                    # If object was not previously found, mark start time
                    object_found = True
                    start_time = current_time_sec
                    # print(f"Đối tượng xuất hiện ở {minutes:02} phút {seconds:02} giây")

        # If object is not in the current frame but was in the previous frames
        if object_found and not object_currently_in_frame:
            object_found = False
            end_time = current_time_sec
            if start_time != end_time:  # Check if start and end times are different
                end_minutes = int(end_time // 60)
                end_seconds = int(end_time % 60)
                start_minutes = int(start_time // 60)
                start_seconds = int(start_time % 60)
                # print(f"Đối tượng xuất hiện từ {start_minutes:02} phút {start_seconds:02} giây đến {end_minutes:02} phút {end_seconds:02} giây")
                result += f"Đối tượng xuất hiện từ {start_minutes:02} phút {start_seconds:02} giây đến {end_minutes:02} phút {end_seconds:02} giây \n"
            start_time = None

        # Display the resulting image
        # cv2.imshow('Video', frame)

        # Hit 'q' on the keyboard to quit
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    # Release handle to the video file
    video_capture.release()
    cv2.destroyAllWindows()
    
    return result

# pathVideo = r'E:\Do An\process-data\data\video_quocbao.mp4'

def tim_kiem_khuon_mat_video(pathImage, pathVideos):
    results = []
    for pathVideo in pathVideos:
        try:
            result = process(pathImage, pathVideo)
            if result != '':
                results.append(result)
            else:
                results.append(False)
        except Exception as e:
            print(f"Lỗi xảy ra khi xử lý video {pathVideo}: {str(e)}")
            results.append(False)
    return results

    