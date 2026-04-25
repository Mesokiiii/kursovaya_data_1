import os
from pathlib import Path

def collect_project_files(root_dir='.', output_file='project_code.txt', extensions=['.cs', '.xaml']):
    """
    Собирает все файлы с указанными расширениями в один текстовый файл
    
    Args:
        root_dir: корневая директория для поиска
        output_file: имя выходного файла
        extensions: список расширений файлов для сбора
    """
    files_found = []
    
    # Исключаем служебные директории
    exclude_dirs = {'bin', 'obj', '.git', '.vs', 'packages', 'node_modules'}
    
    # Ищем все файлы с нужными расширениями
    for ext in extensions:
        for file_path in Path(root_dir).rglob(f'*{ext}'):
            # Проверяем, что файл не в исключенных директориях
            if not any(excluded in file_path.parts for excluded in exclude_dirs):
                files_found.append(file_path)
    
    # Сортируем для удобства
    files_found.sort()
    
    # Записываем в выходной файл
    with open(output_file, 'w', encoding='utf-8') as out:
        out.write(f"=== Собрано файлов: {len(files_found)} ===\n\n")
        
        for file_path in files_found:
            try:
                with open(file_path, 'r', encoding='utf-8') as f:
                    content = f.read()
                
                # Записываем заголовок файла
                out.write(f"\n{'='*80}\n")
                out.write(f"ФАЙЛ: {file_path}\n")
                out.write(f"{'='*80}\n\n")
                
                # Записываем содержимое
                out.write(content)
                out.write("\n\n")
                
            except Exception as e:
                out.write(f"Ошибка чтения файла {file_path}: {e}\n\n")
    
    print(f"✓ Собрано {len(files_found)} файлов в {output_file}")
    for f in files_found:
        print(f"  - {f}")

if __name__ == '__main__':
    # Собираем файлы из основного проекта
    collect_project_files(
        root_dir='project',
        output_file='project_code.txt',
        extensions=['.cs', '.xaml', '.csproj']
    )
    
    # Собираем файлы из тестов
    collect_project_files(
        root_dir='FootballLeague.Tests',
        output_file='tests_code.txt',
        extensions=['.cs', '.csproj']
    )
