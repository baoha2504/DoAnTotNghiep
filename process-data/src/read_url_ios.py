import sqlite3
import datetime

# Hàm chuyển đổi visit_time
def convert_safari_time(timestamp):
    mac_absolute_time = datetime.datetime(2001, 1, 1)
    return mac_absolute_time + datetime.timedelta(seconds=timestamp)

# Đường dẫn đến tệp SQLite
# db_path = r'C:\Users\hacon\Desktop\extract\Library\Safari\History.db'

def read_url_ios(db_path):
    # Kết nối tới cơ sở dữ liệu
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Truy vấn dữ liệu từ bảng history_visits và lấy url từ bảng history_items
    query = """
        SELECT hv.visit_time, hv.title, hi.url
        FROM history_visits hv
        JOIN history_items hi ON hv.history_item = hi.id
        ORDER BY hv.visit_time DESC
    """
    cursor.execute(query)

    # Lấy tất cả các hàng kết quả
    rows = cursor.fetchall()
    url_list = []
    # Hiển thị kết quả với thời gian chuyển đổi
    for row in rows:
        visit_time, title, url = row
        visit_time_converted = convert_safari_time(visit_time)
        # print(f"Visit Time: {visit_time_converted}, Title: {title}, URL: {url}")
        url_dict = {
                "visittime": visit_time_converted,
                "title": title,
                "url": url
            }
        url_list.append(url_dict)
    # Đóng kết nối
    conn.close()
    return url_list
