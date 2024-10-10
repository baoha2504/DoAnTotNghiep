from flask import Flask, jsonify, request
from flask_cors import CORS
from scrape_application import get_info_application
from tim_kiem_khuon_mat_anh import tim_kiem_khuon_mat_anh
from trich_xuat_khuon_mat_anh import trich_xuat_khuon_mat_anh
from tim_kiem_nhieu_khuon_mat_anh import tim_kiem_nhieu_khuon_mat_anh
from tim_kiem_giong_noi_audio import tim_kiem_giong_noi_audio
from tim_kiem_giong_noi_video import tim_kiem_giong_noi_video
from trich_xuat_thong_tin_audio import trich_xuat_thong_tin_audio
from trich_xuat_thong_tin_audio_trong_video import trich_xuat_thong_tin_audio_trong_video
from phan_tich_text_groq import phan_tich_text_groq
from read_file_txt import read_file_txt
from read_file_word import read_file_word
from read_file_pdf import read_file_pdf
from scan_document import scan_document
from ocr_document import ocr_document
from xoay_va_luu_anh import xoay_va_luu_anh
from read_sms_ios import read_sms_ios
from read_callhistory_ios import read_callhistory_ios
from read_calendar_ios import read_calendar_ios
from read_contact_ios import read_contact_ios
from read_url_ios import read_url_ios
from untrack import untrack
from tim_kiem_khuon_mat_video import tim_kiem_khuon_mat_video
from trich_xuat_khuon_mat_video import trich_xuat_khuon_mat_video

app = Flask(__name__)
CORS(app, resources={
    r"/api/*": {"origins": ["http://localhost:3000"]}
})

class DataWeb:
    def __init__(self, tenungdung, duongdananh):
        self.tenungdung = tenungdung
        self.duongdananh = duongdananh

    def to_dict(self):
        return {
            "tenungdung": self.tenungdung,
            "duongdananh": self.duongdananh
        }

def create_DataWeb(tenungdung, duongdananh):
    dataweb = []
    for i in range(len(tenungdung)):
        dataweb.append(DataWeb(tenungdung[i], duongdananh[i]))
    return dataweb


#============================================================================================================
@app.route('/api/GetDataApplication', methods=['POST'])
def GetDataApplication():
    try:
        data = request.json
        if not data or 'packages' not in data:
            return jsonify({"error": "Thiếu tham số 'packages'"}), 400
        
        packages = data['packages']
        if not isinstance(packages, list):
            return jsonify({"error": "'packages' phải là một mảng"}), 400


        tenungdung = []
        duongdananh = []
        
        tenungdung, duongdananh = get_info_application(packages)
        print(f"Thông tin: {tenungdung}, {duongdananh}")

        data_applications = create_DataWeb(tenungdung, duongdananh)
        
        data = [data_application.to_dict() for data_application in data_applications]
        
        return jsonify(data)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500

    
#============================================================================================================
@app.route('/api/TimKiemKhuonMatAnh', methods=['POST'])
def TimKiemKhuonMatAnh():
    try:
        data = request.json
        if not data or 'path' not in data or 'listpath' not in data:
            return jsonify({"error": "Thiếu tham số 'path' hoặc 'listpath'"}), 400
        
        path = data['path']
        listpath = data['listpath']
        
        if not isinstance(listpath, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400

        results = tim_kiem_khuon_mat_anh(path, listpath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500


#============================================================================================================
@app.route('/api/TrichXuatKhuonMatAnh', methods=['POST'])
def TrichXuatKhuonMatAnh():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        listpath = trich_xuat_khuon_mat_anh(path)
        
        return jsonify(listpath)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500


#============================================================================================================
@app.route('/api/TimKiemNhieuKhuonMatAnh', methods=['POST'])
def TimKiemNhieuKhuonMatAnh():
    try:
        data = request.json
        if not data or 'path1' not in data or 'path2' not in data or 'listpath' not in data:
            return jsonify({"error": "Thiếu tham số 'path' hoặc 'listpath'"}), 400
        
        path1 = data['path1']
        path2 = data['path2']
        listpath = data['listpath']
        
        if not isinstance(listpath, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400
        
        results = tim_kiem_nhieu_khuon_mat_anh(path1, path2, listpath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/TimKiemGiongNoiAudio', methods=['POST'])
def TimKiemGiongNoiAudio():
    try:
        data = request.json
        if not data or 'path' not in data or 'listpath' not in data:
            return jsonify({"error": "Thiếu tham số 'path' hoặc 'listpath'"}), 400
        
        path = data['path']
        listpath = data['listpath']
        
        if not isinstance(listpath, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400

        results = tim_kiem_giong_noi_audio(path, listpath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/TimKiemGiongNoiVideo', methods=['POST'])
def TimKiemGiongNoiVideo():
    try:
        data = request.json
        if not data or 'path' not in data or 'listpath' not in data:
            return jsonify({"error": "Thiếu tham số 'path' hoặc 'listpath'"}), 400
        
        path = data['path']
        listpath = data['listpath']
        
        if not isinstance(listpath, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400

        results = tim_kiem_giong_noi_video(path, listpath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/TrichXuatThongTinAudio', methods=['POST'])
def TrichXuatThongTinAudio():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']
        
        results = trich_xuat_thong_tin_audio(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/TrichXuatThongTinAudioTrongVideo', methods=['POST'])
def TrichXuatThongTinAudioTrongVideo():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']
        
        results = trich_xuat_thong_tin_audio_trong_video(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    

#============================================================================================================
@app.route('/api/PhanTichNoiDungTaiLieu', methods=['POST'])
def PhanTichNoiDungTaiLieu():
    try:
        data = request.json
        if not data or 'extension' not in data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'extension' hoặc 'path'"}), 400
        
        extension = data['extension']
        path = data['path']
        text = ''
        if extension == ".doc" or extension == ".docx":
            text = read_file_word(path)
        elif extension == ".txt":
            text = read_file_txt(path)
        elif extension == ".pdf":
            text = read_file_pdf(path)
        
        results = phan_tich_text_groq(text[:5000])
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================
@app.route('/api/ScanDocument', methods=['POST'])
def ScanDocument():
    try:
        data = request.json
        if not data or 'destinationPath' not in data or 'listPath' not in data:
            return jsonify({"error": "Thiếu tham số 'destinationPath' hoặc 'listPath'"}), 400
        
        destinationPath = data['destinationPath']
        listPath = data['listPath']
        
        if not isinstance(listPath, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400

        results = scan_document(destinationPath, listPath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================
@app.route('/api/OCRDocument', methods=['POST'])
def OCRDocument():
    try:
        data = request.json
        if not data or 'filePath' not in data or 'outputPath' not in data:
            return jsonify({"error": "Thiếu tham số 'filePath' hoặc 'outputPath'"}), 400
        
        filePath = data['filePath']
        outputPath = data['outputPath']

        results = ocr_document(filePath, outputPath)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================
@app.route('/api/XoayVaLuuAnh', methods=['POST'])
def XoayVaLuuAnh():
    try:
        data = request.json
        if not data or 'imagepath' not in data or 'targetfolder' not in data or 'rotationdegrees' not in data:
            return jsonify({"error": "Thiếu tham số 'imagepath', 'targetfolder' hoặc 'rotationdegrees'"}), 400
        
        imagepath = data['imagepath']
        targetfolder = data['targetfolder']
        rotationdegrees = data['rotationdegrees']

        results = xoay_va_luu_anh(imagepath, targetfolder, rotationdegrees)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500


#============================================================================================================
@app.route('/api/DocTinNhanIOS', methods=['POST'])
def DocTinNhanIOS():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        results = read_sms_ios(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/DocCuocGoiIOS', methods=['POST'])
def DocCuocGoiIOS():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        results = read_callhistory_ios(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================
@app.route('/api/DocLichIOS', methods=['POST'])
def DocLichIOS():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        results = read_calendar_ios(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================
@app.route('/api/DocDanhBaIOS', methods=['POST'])
def DocDanhBaIOS():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        results = read_contact_ios(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500

#============================================================================================================
@app.route('/api/DocDuongDanIOS', methods=['POST'])
def DocDuongDanIOS():
    try:
        data = request.json
        if not data or 'path' not in data:
            return jsonify({"error": "Thiếu tham số 'path'"}), 400
        
        path = data['path']

        results = read_url_ios(path)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500


#============================================================================================================
@app.route('/api/UntrackBackupIOS', methods=['POST'])
def UntrackBackupIOS():
    try:
        data = request.json
        if not data or 'folder' not in data or 'uuid' not in data:
            return jsonify({"error": "Thiếu tham số 'path' hoặc 'uuid'"}), 400
        
        folder = data['folder']
        uuid = data['uuid']

        results = untrack(folder, uuid)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
    
#============================================================================================================
@app.route('/api/TimKiemKhuonMatVideo', methods=['POST'])
def TimKiemKhuonMatVideo():
    try:
        data = request.json
        if not data or 'pathImage' not in data or 'pathVideos' not in data:
            return jsonify({"error": "Thiếu tham số 'pathImage' hoặc 'pathVideos'"}), 400
        
        pathImage = data['pathImage']
        pathVideos = data['pathVideos']
        
        if not isinstance(pathVideos, list):
            return jsonify({"error": "'listpath' phải là một mảng"}), 400

        results = tim_kiem_khuon_mat_video(pathImage, pathVideos)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500


#============================================================================================================
@app.route('/api/TrichXuatKhuonMatVideo', methods=['POST'])
def TrichXuatKhuonMatVideo():
    try:
        data = request.json
        if not data or 'pathVideo' not in data or 'output_directory' not in data:
            return jsonify({"error": "Thiếu tham số 'pathVideo' hoặc 'output_directory'"}), 400
        
        pathVideo = data['pathVideo']
        output_directory = data['output_directory']

        results = trich_xuat_khuon_mat_video(pathVideo, output_directory)
        print(results)
        
        return jsonify(results)
    except Exception as e:
        print("Lỗi: ", str(e))
        return jsonify({"error": str(e)}), 500
    
#============================================================================================================

#============================================================================================================

#============================================================================================================

#============================================================================================================

#============================================================================================================

#============================================================================================================


if __name__ == '__main__':
    app.run(debug=True, port=5000)
