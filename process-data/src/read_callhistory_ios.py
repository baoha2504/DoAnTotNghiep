import sqlite3
import datetime

# Hàm chuyển đổi visit_time
def convert_safari_time(timestamp):
    mac_absolute_time = datetime.datetime(2001, 1, 1)
    return mac_absolute_time + datetime.timedelta(seconds=timestamp)

# Đường dẫn đến tệp SQLite
# db_path = r'C:\Users\hacon\Desktop\extract\Library\CallHistoryDB\CallHistory.storedata'

def read_callhistory_ios(db_path):
    # Kết nối tới cơ sở dữ liệu
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Truy vấn dữ liệu từ bảng history_visits và lấy url từ bảng history_items
    query = """
        SELECT zh.ZNORMALIZEDVALUE, zc.ZDATE, zc.ZLOCATION
        FROM ZHANDLE zh, ZCALLRECORD zc
        WHERE zh.rowid = zc.rowid
        ORDER BY zc.ZDATE DESC
    """
    cursor.execute(query)

    # Lấy tất cả các hàng kết quả
    rows = cursor.fetchall()

    call_list = []
    # Hiển thị kết quả với thời gian chuyển đổi
    for row in rows:
        znormal, zdate, zlocation = row
        date = convert_safari_time(zdate)
        # print(f"znormal: {znormal}, zdate: {date}, zlocation: {zlocation}")
        
        call_dict = {
                "normal": znormal,
                "date": date,
                "location": zlocation
            }

        # Thêm tin nhắn vào danh sách
        call_list.append(call_dict)
    # Đóng kết nối
    conn.close()
    return call_list
