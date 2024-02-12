[# GB_Chumak_Nikita] https://docs.google.com/document/d/1QzW1oa4CowQEk3SOL4dNw7mfYkolFIj_YB94erhqNd8/edit?usp=sharing
 
Управление: Игрок взаимодействует с игрой благодаря кликам. Есть несколько полей, они различаются цветами.

Красные поля - рука разделенная на позиции игрока/противника. Тут располагаются карты, которые мы можем разыгрывать в течении хода.
Синие поля - поля юнитов разделенные на позиции игрока/противника. Тут располагаются юниты, с которыми игрок может взаимодействовать.
Белые поля - поля священных мест. На этих полях игрок/противник могут возводить храмы.
Желтые поля - поле магазина. С этих полей игрок/противник покупают карты.

Ресурсы: Золото и Вера.
Золото - основной ресурс, за который игрок покупает карты из магазина и возводит храмы.
Вера - одно из условий победы - набрать 40 очков веры. Вера так же тратится на возведение храмов.

Существуют несколько колод. Общая колода(Fold Deck) - находится в центре, рядом с желтыми полями. В эту колоду при старте игры замешиваются 36 карт по 2 копии. Из этой колоды карты перемещаются в магазин.

Колода разыгранных карт(Played Deck) - при активации карты - она перемещается в эту колоду(В дальнейшем будут рассчитываться комбо карт разных конфессий через эту колоду). В конце хода все карты из этой колоды перемещаются в колоду сброса (Discard Deck).

Колода сброса(Discard Deck) - при покупке карт из магазина они помещаются в эту колоду. Колода сброса перемещается в полном составе в колоду добора (Draw Deck), когда колода добора иссякает.

Колода добора (Draw Deck) - из этой колоды игрок получает карты в руку. Изначально в колоду замешивается 10 карт “монетка”, а также по 1 карте бога(планируется грейд карты от алтарей в дальнейшем).

Храмы: Одно из условий победы - контроль над священными местами путем застройки их храмами. Храмы можно активировать и пожертвовать карту из руки, либо с поля юнитов. За пожертвование карты игрок получает награду: из руки - 1 веры, из юнитов - 2 веры. При пожертвовании карты - она удаляется навсегда.

Юниты: Мирные и Военные. Военные юниты могут атаковать вражеские храмы и юнитов. Военные юниты также считаются защитниками. Пока на поле стоит вражеский военный юнит - нельзя атаковать мирных юнитов/храмы. После атаки - юнит отправляется в колоду разыгранных карт.

Типы активации. Карта с типом активации - при покупке из магазина не помещается в колоду сброса, а активируется сразу. Если это юнит - он сразу же размещается на поле юнитов. После смерти юнита с типом активации - он исчезает навсегда.

Добор карт и валюта. В конце каждого хода - игрок сбрасывает все свои оставшиеся карты и оставшееся золота. В начале хода - игрок берёт из колоды добора 6 карт.

Текущий ИИ компьютера: Умеет покупать и разыгрывать карты.

Описание классов и методов:

Класс AudioManager управляет аудио в игре. Он содержит статический экземпляр, который позволяет использовать его как глобальный объект. Этот класс имеет два источника звука: musicSource для фоновой музыки и effectsSource для звуковых эффектов.
Метод Awake() инициализирует синглтон и начинает играть фоновую музыку. Метод OnEnable() подписывается на событие загрузки сцены, чтобы переключать фоновую музыку в зависимости от сцены. Метод InitializeSingleton() создает или уничтожает экземпляр класса, в зависимости от того, является ли он единственным экземпляром.
Метод PlayGameSceneMusic() выбирает и начинает играть музыку в зависимости от текущего выбора игрока. Метод PlayClickSound() воспроизводит звук клика. Метод PlayBackgroundMusic() устанавливает и начинает воспроизводить фоновую музыку. Методы AdjustMusicVolume() и AdjustEffectsVolume() регулируют громкость музыки и эффектов соответственно. Методы ToggleMusic() и ToggleEffects() включают или выключают музыку и эффекты.

Класс BoardPosition управляет позицией на доске в игре. Он содержит информацию о том, занята ли позиция и построен ли в ней храм.
Метод OccupyPosition() занимает позицию указанной картой. Метод VacatePosition() освобождает позицию, удаляя из нее карту. Метод GetCard() возвращает карту, находящуюся в позиции. Метод IsTempleBuilt() проверяет, построен ли в позиции храм.
В этом классе используется приватное свойство IsOccupied, которое отслеживает, занята ли позиция. Также используется приватное свойство cardInPosition для хранения карты, находящейся в позиции.

Класс Card управляет поведением и характеристиками карты в игре. Он содержит свойства, такие как имя карты, тип юнита, тип храма, тип бога, владелец храма, тип исповедания, тип активации и стоимость.
Метод Awake() устанавливает типы карты на основе ее имени, извлекает имя карты и устанавливает стоимость. Метод SetCardCost() устанавливает стоимость карты на основе ее имени. Метод ExtractCardName() извлекает имя карты из имени объекта. Метод SetUsageCount() устанавливает количество использований карты.
Метод ActivateCardEffect() активирует эффект карты, изменяя количество золота или фанатизма у игрока. Метод UpdateCardTextFields() обновляет текстовые поля UI, отображающие информацию о карте.

Класс ClickerBehavior управляет поведением объекта в игре. Он содержит метод ToggleActive(), который включает или выключает объект в зависимости от переданного параметра.
Метод ToggleActive() использует метод SetActive() класса GameObject, чтобы включить или выключить объект. Если параметр isActive равен true, объект становится активным, иначе он становится неактивным.

Класс ClickerManager управляет поведением кликеров в игре. Он содержит массив кликеров и метод ToggleAllClickers(), который включает или выключает все кликеры.
Метод ToggleAllClickers() вызывает событие OnToggleAllClickers, которое может быть обработано другими классами. Метод Awake() подписывается на событие OnToggleAllClickers, а метод OnDestroy() отписывается от него.
Метод HandleToggleAllClickers() обрабатывает событие OnToggleAllClickers. Он проходит по каждому кликеру в массиве и меняет его состояние на противоположное.

Класс ClickHandler, который является частью игры и содержит массив кликеров и метод ToggleAllClickers(). Метод ToggleAllClickers() вызывает событие OnToggleAllClickers, которое может быть обработано другими классами. Метод Awake() подписывается на событие OnToggleAllClickers, а метод OnDestroy() отписывается от него. Метод HandleToggleAllClickers() обрабатывает событие OnToggleAllClickers. Он проходит по каждому кликеру в массиве и меняет его состояние на противоположное.

класс DeckManager, управляет колодами карт в игре. Он содержит массивы для колод игрока и оппонента, а также UI элементы для отображения информации о колодах. Класс также содержит методы для инициализации начальных колод игроков, перемешивания колод, добавления и удаления карт из колод, а также обновления UI текстов, отображающих состояние колод.

класс EnemyTurnManager, управляет ходом противника в игре. Он содержит ссылки на другие менеджеры, такие как HandManager, DeckManager и ClickHandler. Метод StartEnemyTurn() начинает ход противника, активируя все карты в его руке и затем пытаясь купить новые карты из магазина. Корутина BuyCardsForPlayer1() покупает карты для противника, пока у него есть золото.

класс FoldDeck, управляет колодой карт в игре. Он содержит массив префабов карт, список карт в колоде, а также UI элемент для отображения количества карт в колоде. Метод Start() инициализирует колоду, заполняя ее картами и перемешивая их. Метод GetLastCard() возвращает последнюю карту из колоды и удаляет ее из колоды. Метод UpdateFoldDeckText() обновляет UI текст, отображающий количество карт в колоде.

Класс HandManager управляет расположением карт в руках игроков. Он содержит массивы позиций для карт руки первого и второго игрока, а также словари, связывающие позиции с картами. Метод Start() инициализирует начальное состояние рук игроков, устанавливая все позиции как свободные. Методы AddCardToFirstEmptyPlayer0Position() и AddCardToFirstEmptyPlayer1Position() добавляют карту в первую свободную позицию руки соответствующего игрока. Методы DiscardAllPlayer0Cards() и DiscardAllPlayer1Cards() сбрасывают все карты из руки игрока в колоду сброса. Методы GetCardFromPlayer0Position() и GetCardFromPlayer1Position() получают карту из определенной позиции руки игрока. Методы RemoveCardFromPlayer0Position() и RemoveCardFromPlayer1Position() удаляют карту из определенной позиции руки игрока.

класс Highlighter управляет подсветкой различных объектов на игровом поле. Он содержит массивы префабов для различных типов подсветки, а также методы для создания и удаления подсветок. Метод Update() обрабатывает клики мыши и вызывает методы для создания подсветок. Методы HandlePlayerHandHighlight(), HandleSacredSiteHighlight(), HandleStoreHighlight() и HandleUnitHighlight() создают подсветку для соответствующих объектов. Метод HandleNormalHighlight() вызывается при наведении мыши на объект и определяет, какую подсветку нужно создать. Методы ShowOfferingHighlightForAllRelevantPositions() и ShowAttackHighlightForEnemyPositions() создают подсветку для всех соответствующих позиций. Метод RemoveAllHighlights() удаляет все существующие подсветки.

класс InGameMenuManager управляет меню в игре. Он содержит ссылки на объекты панелей игрового меню и меню паузы, а также объект screenlock. Метод OnPauseButtonClicked() вызывается при нажатии кнопки паузы в игровом меню и показывает панель паузы, скрывает игровое меню и открывает screenlock. Метод OnResumeButtonClicked() вызывается при нажатии кнопки возобновления игры в меню паузы и показывает игровое меню, скрывает меню паузы и закрывает screenlock. Метод OnRestartButtonClicked() вызывается при нажатии кнопки перезапуска игры и перезагружает текущую сцену. Метод OnExitButtonClicked() вызывается при нажатии кнопки выхода из игры и загружает главную сцену. Методы OnEffectsButtonClicked() и OnMusicButtonClicked() вызываются при нажатии соответствующих кнопок и переключают звуковые эффекты и музыку соответственно.

класс MainMenuManager управляет меню в игре. Он содержит ссылки на объекты панелей игрового меню, меню паузы и настроек, а также объекты аниматоров и сцены. Методы OnExitButtonClicked(), OnSettingsButtonClicked(), OnPlayButtonClicked(), OnLessMusicVolumeClicked(), OnMoreMusicVolumeClicked(), OnLessEffectsVolumeClicked(), OnMoreEffectsVolumeClicked(), OnSettingsBackClicked(), OnBattleButtonClicked(), OnPlayBackButtonClicked(), OnPreviousConfessionButtonClicked() и OnNextConfessionButtonClicked() вызываются при нажатии соответствующих кнопок и выполняют соответствующие действия. Метод OnBattleBackButtonClicked() вызывается при нажатии кнопки возврата из меню настройки битвы и выполняет соответствующие действия. Метод OnStartBattleButtonClicked() вызывается при нажатии кнопки начала битвы и загружает сцену с индексом 1.

класс ResourcesManager управляет ресурсами игроков в игре. Он содержит ссылки на объекты TextMeshProUGUI для отображения золота и рвения игроков, а также для отображения стоимости храмов. Метод Awake() инициализирует синглтон, а метод Start() обновляет UI при старте игры. Метод UpdateUI() обновляет тексты для отображения золота, рвения и стоимости храмов. Методы ChangePlayer0Gold(), ChangePlayer1Gold(), ChangePlayer0Zeal() и ChangePlayer1Zeal() изменяют ресурсы игроков и автоматически обновляют UI. Метод CheckWinCondition() проверяет условие победы после каждого изменения рвения. Методы UpdatePlayer0TempleCostText() и UpdatePlayer1TempleCostText() обновляют стоимость храмов для каждого игрока. Методы CalculatePlayer0TempleCost() и CalculatePlayer1TempleCost() вычисляют стоимость храмов для каждого игрока.

класс SacredSiteManager, который управляет всеми священными местами в игре. Он содержит статический список sacredSites, который содержит все священные места в игре. Метод OnEnable() подписывается на события добавления и удаления священных мест, а метод OnDisable() отписывается от этих событий. Метод AddSacredSite() добавляет священное место в список, если оно ещё не было добавлено. Метод RemoveSacredSite() удаляет священное место из списка. Метод GetAllSacredSites() возвращает список всех зарегистрированных священных мест.

класс StoreManager управляет магазинами в игре. Он содержит массив storePositions, который содержит позиции магазинов. Метод AddCardToFirstEmptyStore() добавляет карту в первый свободный магазин. Метод GetCardFromStore() получает карту из магазина по индексу.

класс TurnManager управляет ходами игроков в игре. Он содержит ссылки на другие скрипты, которые управляют различными аспектами игры. Метод StartGame() начинает игру с хода игрока. Метод SetCurrentTurn() устанавливает текущий ход игрока. Метод StartPlayer0Turn() начинает ход игрока. Методы EndPlayer0Turn() и EndEnemyTurn() завершают ход игрока или врага соответственно. Метод ProcessEndTurnActions() обрабатывает действия, которые должны быть выполнены после завершения хода игрока или врага. Метод MovePlayedCardsToPlayer0DiscardDeck() перемещает сыгранные карты игрока 0 в его сброс. Метод DiscardAllPlayer0Cards() сбрасывает все карты игрока 0. Метод RefillPlayer0Hand() пополняет руку игрока 0. Метод ChangePlayer0Gold() изменяет количество золота игрока 0. Метод canBuildTemplePlayer0() устанавливает, может ли игрок 0 строить храмы. Метод MovePlayedCardsToPlayer1DiscardDeck() перемещает сыгранные карты игрока 1 в его сброс. Метод DiscardAllPlayer1Cards() сбрасывает все карты игрока 1. Метод RefillPlayer1Hand() пополняет руку игрока 1. Метод ChangePlayer1Gold() изменяет количество золота игрока 1. Метод canBuildTemplePlayer1() устанавливает, может ли игрок 1 строить храмы. Метод UpdateDeckTexts() обновляет тексты колод. Метод EnablePlayer0Interactions() включает или отключает взаимодействия игрока 0.

класс WinLoseConditions, который управляет состоянием игры в зависимости от победы или проигрыша игрока. Он содержит ссылки на объекты интерфейса: меню в игре, блокировку экрана, меню победы и проигрыша. Метод Awake() инициализирует синглтон. Метод CheckWinCondition() проверяет, достигнуты ли условия победы (построенные храмы или очки рвения). Методы PlayerWins() и PlayerLoses() обрабатывают состояние победы и проигрыша соответственно. Метод SetGameEndState() устанавливает состояние интерфейса в зависимости от исхода игры.

