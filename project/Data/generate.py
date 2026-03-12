#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Генератор данных для турнирной таблицы футбольной лиги
Создает JSON файл с 20 командами и 380 матчами (полный сезон)
"""

import json
import random

# Устанавливаем seed для воспроизводимости результатов
random.seed(42)

# Список команд (20 команд)
TEAMS = [
    {"id": 1, "name": "Арсенал"},
    {"id": 2, "name": "Астон Вилла"},
    {"id": 3, "name": "Борнмут"},
    {"id": 4, "name": "Брентфорд"},
    {"id": 5, "name": "Брайтон"},
    {"id": 6, "name": "Челси"},
    {"id": 7, "name": "Кристал Пэлас"},
    {"id": 8, "name": "Эвертон"},
    {"id": 9, "name": "Фулхэм"},
    {"id": 10, "name": "Ливерпуль"},
    {"id": 11, "name": "Лутон Таун"},
    {"id": 12, "name": "Манчестер Сити"},
    {"id": 13, "name": "Манчестер Юнайтед"},
    {"id": 14, "name": "Ньюкасл"},
    {"id": 15, "name": "Ноттингем Форест"},
    {"id": 16, "name": "Шеффилд"},
    {"id": 17, "name": "Тоттенхэм"},
    {"id": 18, "name": "Вест Хэм"},
    {"id": 19, "name": "Вулверхэмптон"},
    {"id": 20, "name": "Бернли"}
]


class Match:
    """Класс для представления матча"""
    def __init__(self, match_id, round_num, home_team_id, away_team_id, home_goals, away_goals):
        self.id = match_id
        self.round = round_num
        self.homeTeamId = home_team_id
        self.awayTeamId = away_team_id
        self.homeGoals = home_goals
        self.awayGoals = away_goals
        self.isPlayed = True
    
    def to_dict(self):
        """Преобразует матч в словарь для JSON"""
        return {
            "id": self.id,
            "round": self.round,
            "homeTeamId": self.homeTeamId,
            "awayTeamId": self.awayTeamId,
            "homeGoals": self.homeGoals,
            "awayGoals": self.awayGoals,
            "isPlayed": self.isPlayed
        }


class MatchGenerator:
    """Класс для генерации матчей"""
    def __init__(self, teams):
        self.teams = teams
        self.matches = []
    
    def generate(self):
        """
        Генерирует 380 матчей для полного сезона
        Каждая команда играет с каждой дома и на выезде
        20 команд * 19 соперников = 380 матчей
        """
        match_id = 1
        team_ids = [team["id"] for team in self.teams]
        
        # Для каждой команды дома
        for home_id in team_ids:
            # Против каждой команды в гостях
            for away_id in team_ids:
                # Пропускаем матч команды с самой собой
                if home_id != away_id:
                    # Генерируем случайный счет
                    home_goals = random.randint(0, 5)
                    away_goals = random.randint(0, 4)
                    
                    # Определяем номер тура (38 туров, по 10 матчей в туре)
                    round_num = ((match_id - 1) // 10) + 1
                    
                    # Создаем матч
                    match = Match(match_id, round_num, home_id, away_id, home_goals, away_goals)
                    self.matches.append(match)
                    match_id += 1
        
        return self.matches


class DataExporter:
    """Класс для экспорта данных в JSON"""
    def __init__(self, teams, matches):
        self.teams = teams
        self.matches = matches
    
    def export_to_json(self, filename):
        """Экспортирует данные в JSON файл"""
        data = {
            "teams": self.teams,
            "matches": [match.to_dict() for match in self.matches]
        }
        
        with open(filename, "w", encoding="utf-8") as f:
            json.dump(data, f, ensure_ascii=False, indent=2)
    
    def get_statistics(self):
        """Возвращает статистику по матчам"""
        home_wins = sum(1 for m in self.matches if m.homeGoals > m.awayGoals)
        away_wins = sum(1 for m in self.matches if m.awayGoals > m.homeGoals)
        draws = sum(1 for m in self.matches if m.homeGoals == m.awayGoals)
        total_goals = sum(m.homeGoals + m.awayGoals for m in self.matches)
        
        return {
            "home_wins": home_wins,
            "away_wins": away_wins,
            "draws": draws,
            "total_goals": total_goals
        }


class DataGenerator:
    """Главный класс для генерации данных"""
    def __init__(self):
        self.teams = TEAMS
        self.generator = MatchGenerator(self.teams)
        self.matches = []
    
    def run(self):
        """Запускает процесс генерации"""
        print("=" * 60)
        print("Генератор данных для турнирной таблицы футбольной лиги")
        print("=" * 60)
        
        # Генерируем матчи
        print("\n[1] Генерирую матчи...")
        self.matches = self.generator.generate()
        print(f"    ✓ Сгенерировано {len(self.matches)} матчей")
        
        # Экспортируем в JSON
        print(f"\n[2] Сохраняю данные в data.json...")
        exporter = DataExporter(self.teams, self.matches)
        exporter.export_to_json("data.json")
        print(f"    ✓ Файл успешно сохранен")
        
        # Выводим статистику
        print("\n[3] Статистика:")
        print(f"    • Команд: {len(self.teams)}")
        print(f"    • Матчей: {len(self.matches)}")
        print(f"    • Туров: {max(m.round for m in self.matches)}")
        
        stats = exporter.get_statistics()
        print(f"\n[4] Результаты матчей:")
        print(f"    • Побед хозяев: {stats['home_wins']}")
        print(f"    • Побед гостей: {stats['away_wins']}")
        print(f"    • Ничьих: {stats['draws']}")
        print(f"    • Всего голов: {stats['total_goals']}")
        
        print("\n" + "=" * 60)
        print("Генерация завершена успешно!")
        print("=" * 60)


# Запуск программы
if __name__ == "__main__":
    generator = DataGenerator()
    generator.run()

