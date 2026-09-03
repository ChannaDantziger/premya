export interface PremiumMethod {
  id: number;
  methodNumber: string;
  description: string;
  premiumRate: number;
  calculationPeriod: string;
}

export interface PremiumMethodRequest {
  methodNumber: string;
  description: string;
  premiumRate: number;
  calculationPeriod: string;
}
