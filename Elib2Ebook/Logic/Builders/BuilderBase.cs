using System;
using System.IO;
using System.Threading.Tasks;
using Elib2Ebook.Configs;
using Elib2Ebook.Extensions;
using Elib2Ebook.Types.Book;

namespace Elib2Ebook.Logic.Builders; 

public abstract class BuilderBase {
    protected readonly Options _options;

    protected abstract string Extension { get;}

    protected BuilderBase(Options options) {
        _options = options;
    }

    /// <summary>
    ///  Создание файла
    /// </summary>
    /// <param name="book">Книга</param>
    protected abstract Task BuildInternal(Book book, string fileName);

    /// <summary>
    /// Получение имени файла
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected virtual string GetFileName(string name) => $"{name}.{Extension}".RemoveInvalidChars();

    /// <summary>
    ///  Создание  файла
    /// </summary>
    /// <param name="book">Книга</param>
    /// <param name="options">Опции</param>
    public async Task Build(Book book) {
        var title = $"{book.Author.Name} - {book.Title}".Crop(100);
        
        var fileName = GetFileName(title);
        
        if (!string.IsNullOrWhiteSpace(_options.SavePath)) {
            if (!Directory.Exists(_options.SavePath)) {
                Directory.CreateDirectory(_options.SavePath);
            }

            fileName = Path.Combine(_options.SavePath, fileName);
        }
        
        Console.WriteLine($"Начинаю сохранение книги {fileName.CoverQuotes()}");
        await BuildInternal(book, fileName);
        
        if (_options.Cover) {
            await SaveCover(_options.SavePath, book.Cover, title);
        }
        
        Console.WriteLine($"Книга {fileName.CoverQuotes()} успешно сохранена");
    }

    /// <summary>
    /// Сохранение обложки книни в отдельный файл
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="cover"></param>
    /// <param name="name"></param>
    private async Task SaveCover(string directory, Image cover, string name) {
        if (cover == null) {
            return;
        }
        
        var fileName = $"{name}_cover.{cover.Extension}".RemoveInvalidChars();

        if (!string.IsNullOrWhiteSpace(directory)) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            fileName = Path.Combine(directory, fileName);
        }

        await using var file = File.OpenWrite(fileName);
        await using var coverStream = cover.GetStream();
        await coverStream.CopyToAsync(file);
    }
}