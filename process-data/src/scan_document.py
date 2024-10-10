from PIL import Image
from reportlab.lib.pagesizes import letter
from reportlab.pdfgen import canvas

def images_to_pdf(output_pdf_path, listPath):
    # Create a canvas object for the PDF
    c = canvas.Canvas(output_pdf_path, pagesize=letter)
    max_width, max_height = letter

    # Loop through each image path
    for img_path in listPath:
        # Open the image using Pillow
        img = Image.open(img_path)

        # Get the width and height of the image
        width, height = img.size

        # Convert the image to RGB mode if it is not
        if img.mode != 'RGB':
            img = img.convert('RGB')

        # Resize the image to fit within the page size if necessary
        if width > max_width or height > max_height:
            img.thumbnail((max_width, max_height))

        # Calculate the position to center the image
        x = (max_width - img.width) / 2
        y = (max_height - img.height) / 2

        # Draw the image on the PDF page at the calculated position
        c.drawImage(img_path, x, y, width=img.width, height=img.height)

        # Add a new page to the PDF
        c.showPage()

    # Save the PDF
    c.save()

def scan_document(destinationPath, listPath):
    images_to_pdf(destinationPath, listPath)
    return destinationPath
