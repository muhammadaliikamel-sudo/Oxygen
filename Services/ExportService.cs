using System.IO.Compression;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class ExportService : IExportService
    {
        private readonly AppDbContext _dbContext;

        public ExportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExportFileResultDto> ExportVisitorsAsync(
            string format,
            Guid? userId,
            CancellationToken cancellationToken)
        {
            var normalizedFormat = NormalizeFormat(format);
            var visitors = await _dbContext.Visitors.ToListAsync(cancellationToken);

            byte[] content;
            string contentType;
            string fileName;

            if (normalizedFormat == "csv")
            {
                var csv = BuildVisitorsCsv(visitors);
                content = Encoding.UTF8.GetBytes(csv);
                contentType = "text/csv";
                fileName = $"visitors-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            }
            else if (normalizedFormat == "xlsx")
            {
                content = BuildXlsx(BuildVisitorsRows(visitors));
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"visitors-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            }
            else if (normalizedFormat == "pdf")
            {
                content = BuildPdf("Visitors Export", BuildVisitorsRows(visitors));
                contentType = "application/pdf";
                fileName = $"visitors-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            }
            else
            {
                throw new NotSupportedException("format is not supported");
            }

            await SaveExportLogAsync(userId, fileName, normalizedFormat, "completed", cancellationToken);

            return new ExportFileResultDto
            {
                Content = content,
                ContentType = contentType,
                FileName = fileName,
                Format = normalizedFormat
            };
        }

        public async Task<ExportFileResultDto> ExportCtasAsync(
            string format,
            Guid? userId,
            CancellationToken cancellationToken)
        {
            var normalizedFormat = NormalizeFormat(format);
            var ctas = await _dbContext.CTAs.ToListAsync(cancellationToken);

            byte[] content;
            string contentType;
            string fileName;

            if (normalizedFormat == "csv")
            {
                var csv = BuildCtasCsv(ctas);
                content = Encoding.UTF8.GetBytes(csv);
                contentType = "text/csv";
                fileName = $"ctas-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            }
            else if (normalizedFormat == "xlsx")
            {
                content = BuildXlsx(BuildCtasRows(ctas));
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"ctas-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            }
            else if (normalizedFormat == "pdf")
            {
                content = BuildPdf("CTAs Export", BuildCtasRows(ctas));
                contentType = "application/pdf";
                fileName = $"ctas-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            }
            else
            {
                throw new NotSupportedException("format is not supported");
            }

            await SaveExportLogAsync(userId, fileName, normalizedFormat, "completed", cancellationToken);

            return new ExportFileResultDto
            {
                Content = content,
                ContentType = contentType,
                FileName = fileName,
                Format = normalizedFormat
            };
        }

        public async Task<ExportFileResultDto> ExportFinalAsync(Guid? userId, CancellationToken cancellationToken)
        {
            var visitorsExport = await ExportVisitorsAsync("csv", userId, cancellationToken);
            var ctasExport = await ExportCtasAsync("csv", userId, cancellationToken);

            await using var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                var visitorsEntry = archive.CreateEntry(visitorsExport.FileName, CompressionLevel.Fastest);
                await using (var entryStream = visitorsEntry.Open())
                {
                    await entryStream.WriteAsync(visitorsExport.Content, cancellationToken);
                }

                var ctasEntry = archive.CreateEntry(ctasExport.FileName, CompressionLevel.Fastest);
                await using (var entryStream = ctasEntry.Open())
                {
                    await entryStream.WriteAsync(ctasExport.Content, cancellationToken);
                }
            }

            var fileName = $"final-export-{DateTime.UtcNow:yyyyMMddHHmmss}.zip";
            await SaveExportLogAsync(userId, fileName, "zip", "completed", cancellationToken);

            return new ExportFileResultDto
            {
                Content = stream.ToArray(),
                ContentType = "application/zip",
                FileName = fileName,
                Format = "zip"
            };
        }

        private async Task SaveExportLogAsync(
            Guid? userId,
            string fileName,
            string format,
            string status,
            CancellationToken cancellationToken)
        {
            var log = new ExportLog
            {
                Id = Guid.NewGuid(),
                UserId = userId ?? Guid.Empty,
                FileName = fileName,
                Format = format,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.ExportLogs.Add(log);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static string NormalizeFormat(string format)
        {
            return string.IsNullOrWhiteSpace(format) ? "csv" : format.Trim().ToLowerInvariant();
        }

        private static string BuildVisitorsCsv(IEnumerable<Visitor> visitors)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,Name,Email,Phone,Company,Category,Source");

            foreach (var visitor in visitors)
            {
                builder.AppendLine(string.Join(',', new[]
                {
                    EscapeCsv(visitor.Id.ToString()),
                    EscapeCsv(visitor.Name),
                    EscapeCsv(visitor.email),
                    EscapeCsv(visitor.Phone),
                    EscapeCsv(visitor.Company),
                    EscapeCsv(visitor.category),
                    EscapeCsv(visitor.Source)
                }));
            }

            return builder.ToString();
        }

        private static List<string[]> BuildVisitorsRows(IEnumerable<Visitor> visitors)
        {
            var rows = new List<string[]>
            {
                new[] { "Id", "Name", "Email", "Phone", "Company", "Category", "Source" }
            };

            foreach (var visitor in visitors)
            {
                rows.Add(new[]
                {
                    visitor.Id.ToString(),
                    visitor.Name,
                    visitor.email,
                    visitor.Phone,
                    visitor.Company,
                    visitor.category,
                    visitor.Source
                });
            }

            return rows;
        }

        private static string BuildCtasCsv(IEnumerable<CTA> ctas)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,InteractionId,Title,Description,Status,Priority,DueDate,Notes,SlaStatus");

            foreach (var cta in ctas)
            {
                builder.AppendLine(string.Join(',', new[]
                {
                    EscapeCsv(cta.Id.ToString()),
                    EscapeCsv(cta.InteractionId.ToString()),
                    EscapeCsv(cta.Title),
                    EscapeCsv(cta.Description),
                    EscapeCsv(cta.Status),
                    EscapeCsv(cta.Priority),
                    EscapeCsv(cta.DueDate.ToString("O")),
                    EscapeCsv(cta.Notes),
                    EscapeCsv(cta.SLAStatus)
                }));
            }

            return builder.ToString();
        }

        private static List<string[]> BuildCtasRows(IEnumerable<CTA> ctas)
        {
            var rows = new List<string[]>
            {
                new[] { "Id", "InteractionId", "Title", "Description", "Status", "Priority", "DueDate", "Notes", "SlaStatus" }
            };

            foreach (var cta in ctas)
            {
                rows.Add(new[]
                {
                    cta.Id.ToString(),
                    cta.InteractionId.ToString(),
                    cta.Title,
                    cta.Description,
                    cta.Status,
                    cta.Priority,
                    cta.DueDate.ToString("O"),
                    cta.Notes,
                    cta.SLAStatus
                });
            }

            return rows;
        }

        private static string EscapeCsv(string value)
        {
            var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
            if (!needsQuotes)
            {
                return value;
            }

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static byte[] BuildXlsx(IReadOnlyList<string[]> rows)
        {
            using var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml());
                AddZipEntry(archive, "_rels/.rels", BuildRootRelsXml());
                AddZipEntry(archive, "xl/workbook.xml", BuildWorkbookXml());
                AddZipEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelsXml());
                AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(rows));
            }

            return stream.ToArray();
        }

        private static byte[] BuildPdf(string title, IReadOnlyList<string[]> rows)
        {
            var lines = new List<string> { title };
            if (rows.Count > 0)
            {
                lines.Add(string.Join(" | ", rows[0]));
            }

            foreach (var row in rows.Skip(1))
            {
                lines.Add(string.Join(" | ", row));
            }

            return SimplePdfBuilder(lines, 46);
        }

        private static byte[] SimplePdfBuilder(IReadOnlyList<string> lines, int maxLines)
        {
            var safeLines = lines.Take(maxLines).Select(EscapePdfText).ToList();
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("BT");
            contentBuilder.AppendLine("/F1 10 Tf");
            contentBuilder.AppendLine("50 780 Td");

            foreach (var line in safeLines)
            {
                contentBuilder.AppendLine($"({line}) Tj");
                contentBuilder.AppendLine("0 -14 Td");
            }

            contentBuilder.AppendLine("ET");
            var content = contentBuilder.ToString();

            var objects = new List<string>
            {
                "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n",
                "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n",
                "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 5 0 R /Resources << /Font << /F1 4 0 R >> >> >>\nendobj\n",
                "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n",
                $"5 0 obj\n<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}endstream\nendobj\n"
            };

            var offsets = new List<int>();
            var pdfBuilder = new StringBuilder();
            pdfBuilder.AppendLine("%PDF-1.4");

            foreach (var obj in objects)
            {
                offsets.Add(Encoding.ASCII.GetByteCount(pdfBuilder.ToString()));
                pdfBuilder.Append(obj);
            }

            var xrefOffset = Encoding.ASCII.GetByteCount(pdfBuilder.ToString());
            pdfBuilder.AppendLine("xref");
            pdfBuilder.AppendLine($"0 {objects.Count + 1}");
            pdfBuilder.AppendLine("0000000000 65535 f ");

            foreach (var offset in offsets)
            {
                pdfBuilder.AppendLine($"{offset:D10} 00000 n ");
            }

            pdfBuilder.AppendLine("trailer");
            pdfBuilder.AppendLine($"<< /Size {objects.Count + 1} /Root 1 0 R >>");
            pdfBuilder.AppendLine("startxref");
            pdfBuilder.AppendLine(xrefOffset.ToString());
            pdfBuilder.AppendLine("%%EOF");

            return Encoding.ASCII.GetBytes(pdfBuilder.ToString());
        }

        private static string EscapePdfText(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private static void AddZipEntry(ZipArchive archive, string path, string content)
        {
            var entry = archive.CreateEntry(path, CompressionLevel.Fastest);
            using var entryStream = entry.Open();
            using var writer = new StreamWriter(entryStream, Encoding.UTF8);
            writer.Write(content);
        }

        private static string BuildContentTypesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                   "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                   "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                   "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                   "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.worksheet+xml\"/>" +
                   "</Types>";
        }

        private static string BuildRootRelsXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                   "</Relationships>";
        }

        private static string BuildWorkbookXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                   "<sheets><sheet name=\"Sheet1\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
                   "</workbook>";
        }

        private static string BuildWorkbookRelsXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                   "</Relationships>";
        }

        private static string BuildWorksheetXml(IReadOnlyList<string[]> rows)
        {
            var builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");

            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var rowNumber = rowIndex + 1;
                builder.Append($"<row r=\"{rowNumber}\">");
                var row = rows[rowIndex];
                for (var colIndex = 0; colIndex < row.Length; colIndex++)
                {
                    var cellRef = GetCellReference(colIndex, rowNumber);
                    var escaped = EscapeXml(row[colIndex]);
                    builder.Append($"<c r=\"{cellRef}\" t=\"inlineStr\"><is><t>{escaped}</t></is></c>");
                }

                builder.Append("</row>");
            }

            builder.Append("</sheetData></worksheet>");
            return builder.ToString();
        }

        private static string GetCellReference(int columnIndex, int rowNumber)
        {
            var dividend = columnIndex + 1;
            var columnName = string.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return $"{columnName}{rowNumber}";
        }

        private static string EscapeXml(string value)
        {
            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}
