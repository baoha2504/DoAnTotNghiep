import requests
from bs4 import BeautifulSoup

def get_info_application(packages):
    tenungdung = []
    duongdananh = []
    
    for url in packages:
        response = requests.get(f"https://apkpure.net/vn/{url}")
        if response.status_code == 200:
            soup = BeautifulSoup(response.content, 'html.parser')

            h1_tag = soup.find('h1')
            h1_text = h1_tag.text if h1_tag else ''
            tenungdung.append(h1_text)
            

            apk_info_div = soup.find('div', {"class": "apk_info"})
            if apk_info_div:
                img_tag = apk_info_div.find('img')
                img_src = img_tag['src'] if img_tag and 'src' in img_tag.attrs else ''
            else:
                img_src = ''
            
            duongdananh.append(img_src)
            
        else:
            duongdananh.append("")
            tenungdung.append("")
            print("Yêu cầu không thành công. Mã trạng thái:", response.status_code)
    
    return tenungdung, duongdananh  