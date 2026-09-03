export interface Metric {
  id: number;
  premiumMethodId: number;
  name: string;
  description: string;
  sourceType: string;
  sourceName: string;
  ingestionFrequency: string;
}

export interface MetricRequest {
  premiumMethodId: number;
  name: string;
  description: string;
  sourceType: string;
  sourceName: string;
  ingestionFrequency: string;
}
