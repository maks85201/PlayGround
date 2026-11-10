# 🛠 Сборка проекта за 30 минут (инструкция для Unity)

## Требования
- Unity Hub + Unity 2022.3 LTS (или новее 2022.x)
- Модуль Android Build Support установлен
- Android SDK / NDK через Unity Hub

## Шаг 1: Создать проект
1. Unity Hub → New project → 2D (Universal Render Pipeline).
2. Projekt folder: `NecromancerCatastrophe`.

## Шаг 2: Скопировать код
Скопируйте папку `Assets/Scripts` из этого репозитория в `Assets/` вашего проекта.

## Шаг 3: Настроить URP 2D
1. `Project Settings` → `Graphics` → Scriptable Render Pipeline Settings → укажите URP Asset.
2. `Project Settings` → `Player` → `Resolution and Presentation` → 
   - Default Orientation: **Portrait**
   - Default Resolution: 1080 x 1920 (или 750 x 1334)
3. `Project Settings` → `Player` → `Other Settings` → `Color Space`: Gamma (для мобильного быстрее).

## Шаг 4: Создать сцены
1. `File` → `New Scene` → сохранить как `Boot` (пустая, с GameManager).
2. `New Scene` → сохранить как `MainMenu`.
   - Canvas → Image (тёмный фон) → кнопка "Начать игру" → `MainMenuController.StartGame()`.
3. `New Scene` → сохранить как `Battle`.
   - Объект "Player" внизу центра: `PlayerCombat`, `PlayerStats`, `NecromancerController`, `SpriteRenderer`, `CircleCollider2D`, `Rigidbody2D` (kinematic).
   - Объект "GameManager": скрипт `GameManager` (DontDestroyOnLoad).
   - Объект "BattleBootstrap": скрипт `BattleBootstrap`, в поле `levelToLoad` назначьте `LevelData` (создайте через правую кнопку → Necromancer Data → Level Data).
   - Объект "SpawnPoint" вверху экрана (Transform) → назначьте в `EnemySpawner.spawnPoint`.
   - Canvas с UI (панель HUD, полосы HP/Мана/Катастрофа, кнопки навыков, кнопка призыва скелета).
   - Объект `EnemySpawner`, `MinionSpawner`, `SoulSystem`, `CatastropheMode`, `RewardSystem`, `UIManager`, `TouchInputController`.
4. `New Scene` → сохранить как `Loading`, `CityHub`, `Settings` (заглушки).

## Шаг 5: Build Settings
1. `File` → `Build Settings`.
2. Добавить сцены: Boot, MainMenu, Battle (в этом порядке).
3. Platform: **Android** → Switch Platform.
4. `Player Settings` → `Identification` → Package Name: `com.yourstudio.necromancercatastrophe`.
5. `Player Settings` → `Publishing Settings` → подпись (Keystore) для релиза.
6. Нажмите **Build** → выбрать папку → получить `.apk`.

## Шаг 6: Запуск на устройстве
- Установите APK на Android через `adb install` или перетащите файл.
- Игра запускается в альбомной/вертикальной depending от настроек Player Settings.

## Программ-арт (первый запуск без спрайтов)
Все объекты создаются как цветные квадраты/круги:
- Герой: голубой неон (0.6, 0.8, 1)
- Враги: красный
- Миньоны: серо-белый
- Души: зелёный круг (0.2, 1, 0.4)
Это позволяет запустить и протестировать механику за 30 минут без внешних ассетов.

## Дальнейшее
- Замените программ-арт на спрайты в `MinionData.prefab`, `EnemyData.prefab`.
- Добавьте анимации через Animator.
- Добавьте звуки через `AudioSource` в `SkillData.castSound`.
- Настройте баланс в `Design/GDD.md` таблицах.
