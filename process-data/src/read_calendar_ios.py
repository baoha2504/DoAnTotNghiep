import sqlite3
import datetime

# Hàm chuyển đổi visit_time
def convert_safari_time(timestamp):
    mac_absolute_time = datetime.datetime(2001, 1, 1)
    return mac_absolute_time + datetime.timedelta(seconds=timestamp)

# Đường dẫn đến tệp SQLite
# db_path = r'C:\Users\hacon\Desktop\extract\Library\Calendar\Calendar.sqlitedb'
def read_calendar_ios(db_path):
    # Kết nối tới cơ sở dữ liệu
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Truy vấn dữ liệu từ bảng history_visits và lấy url từ bảng history_items
    query = """
        SELECT ci.summary, ci.start_date, ci.end_date, it.display_name, it.address
        FROM CalendarItem ci, Identity it
        WHERE ci.availability = it.rowid
        ORDER BY ci.start_date DESC
    """
    cursor.execute(query)

    # Lấy tất cả các hàng kết quả
    rows = cursor.fetchall()
    calendar_list = []
    # Hiển thị kết quả với thời gian chuyển đổi
    for row in rows:
        summary, start_date, end_date, display_name, address = row
        start_date_converted = convert_safari_time(start_date)
        end_date_converted = convert_safari_time(end_date)
        address = address.replace("mailto:", "")

        calendar_dict = {
                "summary": summary,
                "startdateconverted": start_date_converted,
                "enddateconverted": end_date_converted,
                "displayname": display_name,
                "address": address
            }
        calendar_list.append(calendar_dict)
    # Đóng kết nối
    conn.close()
    return calendar_list
