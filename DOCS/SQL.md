# שאילתות SQL מרכזיות

המערכת משתמשת ב־SQLite דרך EF Core. השאילתות הבאות מתארות את השליפות המרכזיות. כל פרמטר חייב להישלח כפרמטר SQL ולא כחיבור מחרוזות.

## שיטות פרמיה

```sql
SELECT Id, MethodNumber, Description, PremiumRate, CalculationPeriod
FROM PremiumMethods
ORDER BY MethodNumber;
```

## מדדים של שיטת פרמיה

```sql
SELECT Id, PremiumMethodId, Name, Description,
       SourceType, SourceName, IngestionFrequency
FROM Metrics
WHERE PremiumMethodId = @PremiumMethodId
ORDER BY Name;
```

## שדות רלוונטיים של מדד

```sql
SELECT Id, MetricId, FieldName, DataType,
       IsRelevant, DisplayOrder
FROM MetricFields
WHERE MetricId = @MetricId
ORDER BY DisplayOrder, Id;
```

## היסטוריית קליטות

```sql
SELECT Id, MetricId, FileStructureVersionId, FileName,
       DataYear, CalculationPeriod, ImportedAt,
       Status, ErrorMessage, RecordCount
FROM ImportBatches
WHERE MetricId = @MetricId
ORDER BY ImportedAt DESC;
```

## שליפת נתונים דינמיים

```sql
SELECT r.Id AS RecordId,
       r.RowNumber,
       f.FieldName,
       f.DataType,
       v.ValueText,
       v.ValueNumber,
       v.ValueDate,
       v.ValueBoolean
FROM DynamicRecords r
JOIN ImportBatches b ON b.Id = r.ImportBatchId
JOIN DynamicValues v ON v.DynamicRecordId = r.Id
JOIN FileStructureFields f ON f.Id = v.FileStructureFieldId
WHERE b.MetricId = @MetricId
  AND (@ImportBatchId IS NULL OR b.Id = @ImportBatchId)
ORDER BY r.RowNumber, f.DisplayOrder
LIMIT @PageSize OFFSET @Offset;
```

## סינון וחיפוש

הסינון מתבצע מול עמודת הערך המתאימה לסוג השדה. בדוגמה הבאה החיפוש הוא טקסטואלי:

```sql
SELECT r.Id AS RecordId, r.RowNumber, v.ValueText
FROM DynamicRecords r
JOIN ImportBatches b ON b.Id = r.ImportBatchId
JOIN DynamicValues v ON v.DynamicRecordId = r.Id
JOIN FileStructureFields f ON f.Id = v.FileStructureFieldId
WHERE b.MetricId = @MetricId
  AND f.FieldName = @FieldName
  AND v.ValueText LIKE @SearchPattern
ORDER BY r.RowNumber
LIMIT @PageSize OFFSET @Offset;
```

## אינדקסים נדרשים

```sql
CREATE INDEX IX_Metrics_PremiumMethodId
    ON Metrics(PremiumMethodId);

CREATE INDEX IX_MetricFields_MetricId
    ON MetricFields(MetricId);

CREATE INDEX IX_ImportBatches_Metric_Period
    ON ImportBatches(MetricId, DataYear, CalculationPeriod);

CREATE INDEX IX_DynamicRecords_ImportBatchId
    ON DynamicRecords(ImportBatchId);

CREATE INDEX IX_DynamicValues_RecordId
    ON DynamicValues(DynamicRecordId);

CREATE INDEX IX_DynamicValues_FieldId
    ON DynamicValues(FileStructureFieldId);
```

## הערה לגבי פרוצדורות

SQLite אינו משתמש בפרוצדורות מאוחסנות כמו SQL Server. לכן במבחן השליפות ממומשות כשאילתות פרמטריות בשכבת ה־Repository, והמעבר העתידי ל־SQL Server יוכל להחליף את ספק הנתונים בלי לשנות את חוזי ה־Application.
