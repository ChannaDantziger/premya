# תכנון API

Base URL מקומי: `http://localhost:5112`

## שיטות פרמיה

| Method | Route | פעולה |
|---|---|---|
| GET | `/api/premium-methods` | קבלת כל השיטות |
| GET | `/api/premium-methods/{id}` | קבלת שיטה לפי מזהה |
| POST | `/api/premium-methods` | יצירת שיטה |
| PUT | `/api/premium-methods/{id}` | עדכון שיטה |

## מדדים ושדות

| Method | Route | פעולה |
|---|---|---|
| GET | `/api/metrics?premiumMethodId={id}` | קבלת מדדי שיטת פרמיה |
| GET | `/api/metrics/{id}` | קבלת מדד לפי מזהה |
| POST | `/api/metrics` | יצירת מדד |
| PUT | `/api/metrics/{id}` | עדכון מדד |
| GET | `/api/metrics/{id}/fields` | קבלת שדות המדד |
| POST | `/api/metrics/{id}/fields` | יצירת שדה למדד |
| PUT | `/api/metrics/fields/{id}` | עדכון שדה |

## קליטות

`POST /api/imports` מקבל `multipart/form-data` הכולל:

- `metricId`
- `dataYear`
- `calculationPeriod`
- `file` בפורמט Excel

| Method | Route | פעולה |
|---|---|---|
| GET | `/api/imports?metricId={id}` | היסטוריית קליטות למדד |
| GET | `/api/imports/{id}` | פרטי קליטה |
| POST | `/api/imports` | קליטת קובץ Excel |

## נתונים דינמיים

`GET /api/metrics/{id}/data` תומך בפרמטרים:

- `importBatchId` — בחירת קליטה ספציפית.
- `fieldName` — שדה לסינון.
- `search` — חיפוש טקסטואלי.
- `sortBy` — שדה למיון.
- `descending` — מיון יורד.
- `page` — מספר עמוד.
- `pageSize` — גודל עמוד.

התשובה כוללת את רשימת השדות, הרשומות, מספר הרשומות הכולל ונתוני עימוד.

## תשתית

| Method | Route | פעולה |
|---|---|---|
| GET | `/api/health` | בדיקת זמינות ה־API |

## תשובות ושגיאות

- `200 OK` — שליפה או פעולה שהושלמה.
- `201 Created` — יצירת שיטה, מדד או שדה.
- `400 Bad Request` — קלט לא תקין או קובץ חסר/לא תקין.
- `404 Not Found` — ישות או מדד לא נמצאו.
- `409 Conflict` — כפילות או הפרת כלל עסקי.

## פער מול דרישת המבחן

במסמך המבחן מופיעה גם דרישה לשליפת “רשימת שדות וסינונים אפשריים”. שליפת השדות ממומשת; endpoint ייעודי ל־filters עדיין דורש השלמה אם נדרש להציגו בנפרד.
