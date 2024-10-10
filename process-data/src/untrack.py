import os
import sqlite3
import shutil

# # Đường dẫn đến folder backup
# backup_folder = r'C:\Users\hacon\Desktop\c15bf3cd0766f2f153c398004ca0e2639b3d72ba'
# # Đường dẫn đến folder xuất file
# output_folder = r'C:\Users\hacon\Desktop\extract'

def untrack(folder, uuid):
    backup_folder = os.path.join(folder, uuid)
    output_folder = os.path.join(folder, "extract")
    manifest_db_path = os.path.join(backup_folder, 'Manifest.db')

    # Kết nối với cơ sở dữ liệu Manifest.db
    conn = sqlite3.connect(manifest_db_path)
    cursor = conn.cursor()

    # Truy vấn để lấy đường dẫn của các file cần trích xuất
    query = """
    SELECT fileID, relativePath 
    FROM Files 
    WHERE relativePath LIKE '%.jpg' OR relativePath LIKE '%.jpeg' 
    OR relativePath LIKE '%.png' OR relativePath LIKE '%.mp4'
    OR relativePath LIKE '%.m4a' OR relativePath LIKE '%.mp3' 
    OR relativePath LIKE '%.wav' OR relativePath LIKE '%.doc' 
    OR relativePath LIKE '%.docx' OR relativePath LIKE '%.xls' 
    OR relativePath LIKE '%.xlsx' OR relativePath LIKE '%.ppt' 
    OR relativePath LIKE '%.pptx' OR relativePath LIKE '%.pdf'
    OR relativePath LIKE '%.txt' OR relativePath LIKE '%.db' 
    OR relativePath LIKE '%.sqlite' OR relativePath LIKE '%.sqlite3'
    OR relativePath LIKE '%.storedata' OR relativePath LIKE '%.sqlitedb'
    OR relativePath LIKE '%.dbsqlite'
    """
    cursor.execute(query)
    files = cursor.fetchall()

    # Đóng kết nối với cơ sở dữ liệu
    conn.close()

    # Tạo thư mục đầu ra nếu chưa tồn tại
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    for file_id, relative_path in files:
        try:
            # Đường dẫn đến file đã backup
            backup_file_path = os.path.join(backup_folder, file_id[:2], file_id)
            
            # Đường dẫn đầy đủ của file đầu ra
            output_file_path = os.path.join(output_folder, relative_path)
            
            # Tạo thư mục đầu ra nếu chưa tồn tại
            os.makedirs(os.path.dirname(output_file_path), exist_ok=True)
            
            # Sao chép file từ backup ra thư mục đầu ra
            shutil.copy2(backup_file_path, output_file_path)
            
            print(f"Extracted: {relative_path}")
            
        except Exception as e:
            print(f"Error extracting {relative_path}: {e}")
            continue
    
    return output_folder

# untrack(r'C:\Users\hacon\AppData\Roaming\iMazing\Backups', '00008020-00055D041A08002E')
