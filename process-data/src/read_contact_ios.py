import sqlite3
import datetime

# Hàm chuyển đổi visit_time
def convert_safari_time(timestamp):
    mac_absolute_time = datetime.datetime(2001, 1, 1)
    return mac_absolute_time + datetime.timedelta(seconds=timestamp)

# Đường dẫn đến tệp SQLite
# db_path = r'C:\Users\hacon\Desktop\extract\Library\AddressBook\AddressBook.sqlitedb'
def read_contact_ios(db_path):
    # Kết nối tới cơ sở dữ liệu
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Truy vấn dữ liệu từ bảng history_visits và lấy url từ bảng history_items
    query = """
        SELECT abp.Last, abp.CreationDate, abp.ModificationDate, abm.value
        FROM ABPerson abp, ABMultiValue abm
        WHERE abp.ROWID = abm.record_id
        ORDER BY abp.CreationDate DESC
    """
    cursor.execute(query)

    # Lấy tất cả các hàng kết quả
    rows = cursor.fetchall()
    contact_list = []

    # Hiển thị kết quả với thời gian chuyển đổi
    for row in rows:
        Last, CreationDate, ModificationDate, value = row
        CreationDate_converted = convert_safari_time(CreationDate)
        ModificationDate_converted = convert_safari_time(ModificationDate)
        # print(f"Name: {Last}, CreationDate: {CreationDate_converted}, ModificationDate: {ModificationDate_converted}, value: {value}")
        calendar_dict = {
                "name": Last,
                "creationdate": CreationDate_converted,
                "modificationdate": ModificationDate_converted,
                "value": value
            }
        contact_list.append(calendar_dict)
    # Đóng kết nối
    conn.close()
    return contact_list
