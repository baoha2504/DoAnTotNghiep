from flask import Flask, request, jsonify
from flask_cors import CORS
from src import create_db
from src import insert_db
from src import select_db
import json
import os
import datetime


app = Flask(__name__)

# Chỉ cho phép một số địa chỉ IP và cổng cụ thể
cors = CORS(app, resources={
    r"/*": {
        "origins": ["http://192.168.43.107:5000", "http://localhost:5000"]
    }
})

@app.route('/login', methods=['POST'])
def login_account():
    data = request.json

    if not data or 'username' not in data or 'password' not in data:
        return jsonify({'error': 'Username and password are required'}), 400

    username = data.get('username')
    password = data.get('password')

    result = select_db.select_account_by_username_password(username, password)

    if result:
        account_id = result[0] 
        return jsonify({'message': 'Login successful', 'account_id': account_id}), 200
    else:
        return jsonify({'error': 'Invalid credentials'}), 401


@app.route('/loginwithpin', methods=['POST'])
def login_pin():
    try:
        data = request.json

        if not data or 'device_serial' not in data or 'device_name' not in data or 'pincode' not in data or 'account_id' not in data:
            return jsonify({'error': 'data are required'}), 400

        device_serial = data.get('device_serial')
        device_name = data.get('device_name')
        pincode = data.get('pincode')
        account_id = data.get('account_id')

        current_time = datetime.datetime.now()
        insert_db.add_login_history(current_time, device_serial, device_name, pincode, account_id)

        return jsonify({'message': 'Login pin successful'}), 200
    except Exception:
        return jsonify({'error': 'Invalid credentials'}), 401


@app.route('/updatecountstarttimelast', methods=['POST'])
def update_count_starttime_last():
    try:
        data = request.json

        if not data or 'account_id' not in data:
            return jsonify({'error': 'data are required'}), 400

        account_id = data.get('account_id')

        current_time = datetime.datetime.now()
        insert_db.update_count_coin(account_id, count_starttime_last=current_time)

        return jsonify({'message': 'update count starttime last successful'}), 200
    except Exception:
        return jsonify({'error': 'Invalid credentials'}), 401


@app.route('/GetAllAccount', methods=['POST'])
def get_all_account():
    try:
        data = request.json

        if not data or 'account_id' not in data:
            return jsonify({'error': 'data are required'}), 400

        result = select_db.select_all_account()
        
        return jsonify(result), 200
    except Exception:
        return jsonify({'error': 'Invalid credentials'}), 401


@app.route('/GetAllLoginHistory', methods=['POST'])
def get_all_login_history():
    try:
        data = request.json

        if not data or 'account_id' not in data:
            return jsonify({'error': 'data are required'}), 400

        result = select_db.select_all_login_history()
        
        return jsonify(result), 200
    except Exception:
        return jsonify({'error': 'Invalid credentials'}), 401

@app.route('/GetLoginHistoryByAccountID', methods=['POST'])
def get_login_history_by_account_id():
    try:
        data = request.json

        if not data or 'account_id' not in data:
            return jsonify({'error': 'data are required'}), 400

        account_id = data.get('account_id')
        
        result = select_db.select_login_history_by_account_id(account_id)
        
        return jsonify(result), 200
    except Exception:
        return jsonify({'error': 'Invalid credentials'}), 401


@app.route('/ListenerClient', methods=['POST'])
def listener_client():
    try:
        data = request.json

        if not data or not isinstance(data, list):
            return jsonify({'error': 'A list of call logs is required'}), 400

        
        filtered_data = []

        for log in data:
            filtered_record = {
                'row': log.get('row'),
                'number': log.get('number'),
                'type': log.get('type'),
                'date': log.get('date'),
                'duration': log.get('duration'),
            }
            filtered_data.append(filtered_record)

        device_name = data[0].get('device_name')

        file_path = os.path.join(os.getcwd(), f'data/call_logs_{device_name}.json')

        with open(file_path, 'w', encoding='utf-8') as json_file:
            json.dump(filtered_data, json_file, ensure_ascii=False, indent=4)

        print(f"Export success file: {file_path}")
        return jsonify({"message": "Send success"}), 200
    
    except KeyError as e:
        return jsonify({'error': f'Missing field: {str(e)}'}), 400
    
    except ValueError as e:
        return jsonify({'error': f'Invalid value: {str(e)}'}), 400
    
    except Exception as e:
        return jsonify({'error': f'An error occurred: {str(e)}'}), 500


if __name__ == '__main__':
    app.run(host='192.168.43.107', port=5001, debug=True)