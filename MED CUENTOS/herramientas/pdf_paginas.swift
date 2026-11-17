// Extrae el texto de cada página de un PDF y lo imprime como JSON.
// Usa PDFKit y, si la página no tiene texto (solo imagen), OCR con Vision.
// Uso: swift pdf_paginas.swift <archivo.pdf>
import Foundation
import PDFKit
import Vision
import AppKit

func ocr(_ page: PDFPage) -> String {
    let box = page.bounds(for: .mediaBox)
    let scale: CGFloat = 2.0
    let size = NSSize(width: box.width * scale, height: box.height * scale)
    let image = page.thumbnail(of: size, for: .mediaBox)
    guard let cg = image.cgImage(forProposedRect: nil, context: nil, hints: nil) else { return "" }
    let request = VNRecognizeTextRequest()
    request.recognitionLevel = .accurate
    request.recognitionLanguages = ["es-ES", "en-US"]
    request.usesLanguageCorrection = true
    try? VNImageRequestHandler(cgImage: cg, options: [:]).perform([request])
    let obs = (request.results ?? []).sorted { $0.boundingBox.minY > $1.boundingBox.minY }
    return obs.compactMap { $0.topCandidates(1).first?.string }.joined(separator: "\n")
}

guard CommandLine.arguments.count > 1,
      let doc = PDFDocument(url: URL(fileURLWithPath: CommandLine.arguments[1])) else {
    FileHandle.standardError.write("No se pudo abrir el PDF\n".data(using: .utf8)!)
    exit(1)
}

var pages: [[String: Any]] = []
for i in 0..<doc.pageCount {
    guard let page = doc.page(at: i) else { continue }
    var text = (page.string ?? "").replacingOccurrences(of: "\u{FFFC}", with: "")
        .trimmingCharacters(in: .whitespacesAndNewlines)
    var method = "texto"
    if text.isEmpty {
        text = ocr(page).trimmingCharacters(in: .whitespacesAndNewlines)
        method = text.isEmpty ? "vacia" : "ocr"
    }
    pages.append(["page": i + 1, "method": method, "text": text])
}
let data = try JSONSerialization.data(withJSONObject: pages, options: [.prettyPrinted])
print(String(data: data, encoding: .utf8)!)
