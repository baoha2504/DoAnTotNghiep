import face_recognition
import cv2
import os


# Function to save the entire frame when a new face is detected and draw a red square around it
def save_frame(image, index, face_location, output_directory):
    top, right, bottom, left = face_location
    # Draw a red rectangle around the face
    cv2.rectangle(image, (left, top), (right, bottom), (0, 0, 255), 2)
    # Save the image with the drawn rectangle
    frame_filename = os.path.join(output_directory, f"face_{index}.jpg")
    cv2.imwrite(frame_filename, image)
    print(f"New frame saved: {frame_filename}")
    return frame_filename
    


def process(pathVideo, output_directory):
    result = []
    # Create the output directory if it doesn't exist
    if not os.path.exists(output_directory):
        os.makedirs(output_directory)

    video_capture = cv2.VideoCapture(pathVideo)

    # Known face encodings and names (initially empty)
    known_face_encodings = []
    known_face_names = []

    # Initialize some variables
    face_locations = []
    face_encodings = []
    face_names = []
    frame_counter = 0
    fps = 3  # Process n frames per second
    new_face_index = 0
    

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

        # Process only 2 frames per second (fps)
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
        for face_encoding, face_location in zip(face_encodings, face_locations):
            # Check if this face is a match for any known face
            matches = face_recognition.compare_faces(known_face_encodings, face_encoding, tolerance=0.5)
            name = "Unknown"

            # If a match was found in known_face_encodings
            if True in matches:
                first_match_index = matches.index(True)
                name = known_face_names[first_match_index]
            else:
                # Scale back up face location before saving
                scaled_face_location = tuple([coord * 4 for coord in face_location])
                # Save the entire frame if a new face is detected and draw a square around it
                frame_filename = save_frame(frame, new_face_index, scaled_face_location, output_directory)
                result.append(frame_filename)
                known_face_encodings.append(face_encoding)
                known_face_names.append(f"New Face {new_face_index}")
                new_face_index += 1

            face_names.append(name)

        # Display the results
        for (top, right, bottom, left), name in zip(face_locations, face_names):
            # Scale back up face locations since the frame we detected in was scaled to 1/4 size
            top *= 4
            right *= 4
            bottom *= 4
            left *= 4

            # Draw a box around the face
            cv2.rectangle(frame, (left, top), (right, bottom), (0, 0, 255), 2)

            # Draw a label with a name below the face
            cv2.rectangle(frame, (left, bottom - 35), (right, bottom), (0, 0, 255), cv2.FILLED)
            font = cv2.FONT_HERSHEY_DUPLEX
            cv2.putText(frame, name, (left + 6, bottom - 6), font, 1.0, (255, 255, 255), 1)

        # Display the resulting image
        # cv2.imshow('Video', frame)

        # Hit 'q' on the keyboard to quit!
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    # Release handle to the video file
    video_capture.release()
    cv2.destroyAllWindows()
    return result
    
# Load the video file
# pathVideo = r'E:\Do An\process-data\data\CameraRoll\WIN_20240912_15_39_38_Pro.mp4'
# output_directory = r'E:\Do An\process-data\data\CameraRoll\Face'

def trich_xuat_khuon_mat_video(pathVideo, output_directory):
    try:
        result = process(pathVideo, output_directory)
        if len(result) > 0:
            return result
        else:
            return result
    except Exception as e:
        print(f"Lỗi xảy ra khi trích xuất khuôn mặt từ video {pathVideo}: {str(e)}")
        return result 
