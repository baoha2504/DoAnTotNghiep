import sqlite3
import datetime

# Hàm chuyển đổi timestamp cho trường `date` và `date_read`
# Hàm chuyển đổi timestamp cho trường `date` và `date_read`
def convert_safari_time1(timestamp, unit='nano'):
    try:
        if unit == 'nano':
            return datetime.datetime(2001, 1, 1) + datetime.timedelta(seconds=timestamp / 1_000_000_000)
        elif unit == 'micro':
            return datetime.datetime(2001, 1, 1) + datetime.timedelta(microseconds=timestamp)
        elif unit == 'milli':
            return datetime.datetime(2001, 1, 1) + datetime.timedelta(milliseconds=timestamp)
        else:
            return datetime.datetime(2001, 1, 1) + datetime.timedelta(seconds=timestamp)
    except OverflowError:
        return None

def convert_safari_time2(timestamp):
    try:
        # Thời gian gốc là 1/1/2001
        mac_absolute_time = datetime.datetime(2001, 1, 1)
        return mac_absolute_time + datetime.timedelta(seconds=timestamp)
    except OverflowError:
        return None

    # # Đường dẫn đến tệp SQLite
    # 
def read_sms_ios(db_path):
    # Kết nối tới cơ sở dữ liệu
    # db_path = r'C:\Users\hacon\Desktop\extract\Library\SMS\sms.db'
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Truy vấn dữ liệu từ các cột text, service, account, date, date_read
    query = """
        SELECT mes.text, mes.service, han.id, mes.date, mes.date_read, mes.handle_id 
        FROM message mes, handle han
        WHERE mes.handle_id = han.ROWID
        ORDER BY mes.date DESC
    """
    cursor.execute(query)

    # Lấy tất cả các hàng kết quả
    rows = cursor.fetchall()
    
    # Tạo một danh sách để lưu trữ các tin nhắn dưới dạng từ điển
    sms_list = []

    # Thử nghiệm với các đơn vị thời gian khác nhau
    for row in rows:
        text, service, id, date, date_read, handle_id = row
        date_converted = convert_safari_time1(date, unit='nano')  # Xử lý `date` là nanosecond
        date_read_converted = convert_safari_time2(date_read)  # Xử lý `date_read` là giây
        # print(f"Text: {text}, Service: {service}, Account: {id}, Date: {date_converted}, Date Read: {date_read_converted}")
        # Tạo từ điển cho từng tin nhắn
        sms_dict = {
            "text": text,
            "service": service,
            "destinationcaller": id,
            "date": date_converted,
            "dateread": date_read_converted,
            "handle": handle_id,
        }

        # Thêm tin nhắn vào danh sách
        sms_list.append(sms_dict)
    # Đóng kết nối
    conn.close()
    return sms_list
