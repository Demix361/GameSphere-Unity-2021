from translate import Translator
from prettytable import PrettyTable
x = PrettyTable()

x.field_names = ['ar',
    'zh',
    'en',
    'fr',
    'de',
    'it',
    'ja',
    'ko',
    'pl',
    'pt',
    'ru',
    'es',
    'th',
    'tr',
    'uk' ]

text = [
    'Music Volume',
    'Effects Volume',
    'Reset Progress',
    'Return',
    'Settings',
    'Language'
]

languages = [
    'ar',
    'zh',
    'fr',
    'de',
    'it',
    'ja',
    'ko',
    'pl',
    'pt',
    'ru',
    'es',
    'th',
    'tr',
    'uk' 
]

translators = []
for lang in languages:
    translators.append(Translator(to_lang=lang))

res = []
for tr in translators:
    res.append([])
    for j in range(len(text)):
        res[-1].append(tr.translate(text[j]))
print(res)

for i in range(len(text)):
    row = []
    for j in range(len(languages) + 1):
        if j < 2:
            row.append(res[j][i])
        elif j == 2:
            row.append(text[i])
        else:
            row.append(res[j - 1][i])
    x.add_row(row)
print(x)
