(() => {
    const translations = {
        ru: {
            'nav.saved': 'Мои анализы', 'nav.new': 'Новый анализ', 'nav.theory': 'Теория',
            'home.eyebrow': 'Математическая тетрадь', 'home.title': 'Исследование функций',
            'home.lead': 'Введите формулу и получите производную, нули, критические точки, экстремумы и наглядный график в одном месте.',
            'home.start': 'Начать анализ', 'home.learn': 'Изучить теорию',
            'home.fastTitle': 'Быстрый анализ', 'home.fastText': 'Находите основные свойства функции одним вводом и сохраняйте результат.',
            'home.graphTitle': 'Понятный график', 'home.graphText': 'Сравнивайте функцию и её производную на одной координатной плоскости.',
            'home.exampleTitle': 'Примеры рядом', 'home.exampleText': 'Начните с готового примера или введите собственную формулу.',
            'create.eyebrow': 'Новое исследование', 'create.title': 'Исследуйте функцию', 'create.lead': 'Выберите пример или введите собственную формулу.',
            'create.examples': 'Пример функции', 'create.formula': 'Формула функции', 'create.save': 'Анализировать и сохранить', 'create.back': 'Вернуться к списку',
            'create.keypadTitle': 'Калькулятор функций', 'create.keypadLead': 'Составляйте формулу с помощью кнопок.',
            'create.hint': 'Поддерживаются <code>+ − × ÷ ^</code>, <code>x</code>, <code>pi</code>, <code>e</code>, а также, например, <code>sin(x)</code>, <code>sqrt(x)</code> и <code>ln(x)</code>.', 'create.choose': '— Выберите пример (необязательно) —', 'create.placeholder': 'Например: x^2 - 4*x + 3',
            'index.eyebrow': 'Ваша история', 'index.title': 'Мои анализы', 'index.new': '+ Новый анализ',
            'index.empty': 'Сохранённых функций пока нет.', 'table.formula': 'Формула', 'table.derivative': 'Производная', 'table.zeros': 'Нули', 'table.extrema': 'Экстремумы', 'table.view': 'Открыть', 'table.edit': 'Изменить', 'table.delete': 'Удалить',
            'details.domain': 'Область определения', 'details.derivative': 'Производная', 'details.zeros': 'Нули', 'details.critical': 'Критические точки', 'details.extrema': 'Экстремумы',
            'details.range': 'Диапазон графика: от −10 до 10.', 'details.edit': 'Изменить', 'details.back': 'Назад',
            'edit.title': 'Изменить функцию', 'edit.formula': 'Формула функции', 'edit.save': 'Сохранить', 'edit.back': 'Назад',
            'theory.tocDomain': '01   Область определения', 'theory.tocZeros': '02   Нули функции', 'theory.tocDerivative': '03   Производная', 'theory.tocCritical': '04   Критические точки', 'theory.tocExtrema': '05   Экстремумы', 'theory.tocShape': '06   Форма графика',
            'theory.eyebrow': 'Краткий справочник', 'theory.title': 'Исследование функции,<br>спокойно и по порядку.', 'theory.lead': 'Используйте эту страницу рядом с анализом формулы. Каждый раздел помогает понять, что означает результат.'
        },
        et: {}
    };
    const originalTheory = document.querySelector('.theory-content')?.innerHTML;
    const theoryRussian = `
        <section class="theory-section" id="domain"><span>01</span><h2>Область определения</h2><p>Область определения показывает, какие значения <em>x</em> допустимо подставлять в формулу. На ноль делить нельзя, подкоренное выражение должно быть неотрицательным, а выражение внутри логарифма — положительным.</p><div class="theory-example"><b>Пример</b><p>Для <code>f(x) = 1 / (x − 2)</code> значение <em>x = 2</em> не допускается, потому что знаменатель был бы равен нулю.</p></div></section>
        <section class="theory-section" id="zeros"><span>02</span><h2>Нули функции</h2><p>В нулях значение функции равно нулю — график пересекает или касается оси x. Чтобы их найти, решите уравнение <code>f(x) = 0</code>.</p><div class="theory-example"><b>Пример</b><p><code>x² − 4 = 0</code> даёт нули <em>x = −2</em> и <em>x = 2</em>.</p></div></section>
        <section class="theory-section" id="derivative"><span>03</span><h2>Первая производная</h2><p>Производная показывает, как функция изменяется в данный момент. Если <code>f′(x) &gt; 0</code>, функция возрастает; если <code>f′(x) &lt; 0</code>, убывает.</p><div class="theory-example"><b>Полезное правило</b><p><code>(xⁿ)′ = n · xⁿ⁻¹</code>. Например, <code>(x³)′ = 3x²</code>.</p></div></section>
        <section class="theory-section" id="critical"><span>04</span><h2>Критические точки</h2><p>Критическая точка может находиться там, где <code>f′(x) = 0</code>, или производная не существует, но сама функция определена. Эти точки делят график на интервалы возрастания и убывания.</p></section>
        <section class="theory-section" id="extrema"><span>05</span><h2>Экстремумы</h2><p>Если знак производной меняется с плюса на минус, это локальный максимум. Если с минуса на плюс — локальный минимум.</p><div class="theory-example"><b>Пример</b><p>У функции <code>x² − 4</code> минимум находится в точке <em>(0; −4)</em>.</p></div></section>
        <section class="theory-section" id="shape"><span>06</span><h2>Форма графика</h2><p>Вторая производная помогает увидеть изгиб. Если <code>f″(x) &gt; 0</code>, график направлен вверх; если <code>f″(x) &lt; 0</code>, вниз. Смена знака может означать точку перегиба.</p></section>`;
    function applyLanguage(language) {
        document.documentElement.lang = language;
        localStorage.setItem('language', language);
        document.getElementById('languageSelect').value = language;
        document.querySelectorAll('[data-i18n]').forEach(element => {
            const value = translations[language]?.[element.dataset.i18n];
            if (value) element.innerHTML = value;
        });
        const content = document.querySelector('.theory-content');
        if (content && originalTheory) content.innerHTML = language === 'ru' ? theoryRussian : originalTheory;
        document.querySelectorAll('.results span').forEach(element => {
            if (!element.dataset.estonianText) element.dataset.estonianText = element.textContent;
            let text = element.dataset.estonianText;
            if (language === 'ru') text = text.replaceAll('puuduvad', 'нет').replaceAll('maksimum', 'максимум').replaceAll('miinimum', 'минимум').replaceAll('R (reaalarvud)', 'R (действительные числа)').replaceAll('R välja arvatud ligikaudsed katkestuskohad:', 'R, кроме примерных точек разрыва:');
            element.textContent = text;
        });
        document.querySelectorAll('[data-i18n-placeholder]').forEach(element => {
            const value = translations[language]?.[element.dataset.i18nPlaceholder];
            if (value) element.placeholder = value;
        });
        window.dispatchEvent(new CustomEvent('languagechange', { detail: { language } }));
    }
    document.addEventListener('DOMContentLoaded', () => {
        const select = document.getElementById('languageSelect');
        if (!select) return;
        applyLanguage(localStorage.getItem('language') || 'et');
        select.addEventListener('change', () => applyLanguage(select.value));
    });
})();
