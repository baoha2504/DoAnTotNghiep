import sqlite3
from db.settings import settings

# Hàm thêm dữ liệu vào bảng account
def add_account(displayname, username, password):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO account (displayname, username, password)
        VALUES (?, ?, ?)
    ''', (displayname, username, password))
    conn.commit()
    conn.close()
    print("Account added successfully.")

# Hàm cập nhật dữ liệu trong bảng account
def update_account(account_id, displayname=None, username=None, password=None):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    if displayname:
        cursor.execute('UPDATE account SET displayname = ? WHERE account_id = ?', (displayname, account_id))
    if username:
        cursor.execute('UPDATE account SET username = ? WHERE account_id = ?', (username, account_id))
    if password:
        cursor.execute('UPDATE account SET password = ? WHERE account_id = ?', (password, account_id))
    conn.commit()
    conn.close()
    print("Account updated successfully.")


################################################################################################

# Hàm thêm dữ liệu vào bảng login_history
def add_login_history(date_time, device_serial, device_name, pincode, account_id):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO login_history (date_time, device_serial, device_name, pincode, account_id)
        VALUES (?, ?, ?, ?, ?)
    ''', (date_time, device_serial, device_name, pincode, account_id))
    conn.commit()
    conn.close()
    print("Login history added successfully.")

# Hàm cập nhật dữ liệu trong bảng login_history
def update_login_history(login_history_id, date_time=None, device_serial=None, device_name=None, pincode=None, account_id=None):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    if date_time:
        cursor.execute('UPDATE login_history SET date_time = ? WHERE login_history_id = ?', (date_time, login_history_id))
    if device_serial:
        cursor.execute('UPDATE login_history SET device_serial = ? WHERE login_history_id = ?', (device_serial, login_history_id))
    if device_name:
        cursor.execute('UPDATE login_history SET device_name = ? WHERE login_history_id = ?', (device_name, login_history_id))
    if pincode:
        cursor.execute('UPDATE login_history SET pincode = ? WHERE login_history_id = ?', (pincode, login_history_id))
    if account_id:
        cursor.execute('UPDATE login_history SET account_id = ? WHERE login_history_id = ?', (account_id, login_history_id))
    conn.commit()
    conn.close()
    print("Login history updated successfully.")

################################################################################################

# Hàm thêm dữ liệu vào bảng count_coin
def add_count_coin(count_start, count_starttime_last, current_coin_count, account_id):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO count_coin (count_start, count_starttime_last, current_coin_count, account_id)
        VALUES (?, ?, ?, ?)
    ''', (count_start, count_starttime_last, current_coin_count, account_id))
    conn.commit()
    conn.close()
    print("Count coin added successfully.")

# Hàm cập nhật dữ liệu trong bảng count_coin
def update_count_coin(account_id, count_coin_id=None, count_start=None, count_starttime_last=None, current_coin_count=None):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    if count_start:
        cursor.execute('UPDATE count_coin SET count_start = ? WHERE account_id = ?', (count_start, account_id))
    if count_starttime_last:
        cursor.execute('UPDATE count_coin SET count_starttime_last = ? WHERE account_id = ?', (count_starttime_last, account_id))
    if current_coin_count:
        cursor.execute('UPDATE count_coin SET current_coin_count = ? WHERE account_id = ?', (current_coin_count, account_id))
    conn.commit()
    conn.close()
    print("Count coin updated successfully.")

################################################################################################

# add_account('John Doe', 'johndoe', 'password123')
# update_account(1, displayname='John Smith', password='newpassword123')

# add_login_history('2024-09-12 10:00:00', '123ABC', 'iPhone 12', '1234', 1)
# update_login_history(1, device_name='iPhone 13', pincode='5678')

# add_count_coin('2024-09-12 08:00:00', '2024-09-12 09:00:00', 500, 1)
# update_count_coin(1, current_coin_count=600)