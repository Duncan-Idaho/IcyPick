export interface Data {
  heroes: Hero[];
  maps: Map[];
}

export interface Map {
  id: string;
  name: string;
}

export interface Hero {
  id: string;
  name: string;
  role: string;
  guideUri: string;
  mapPreference: HeroMapPreference;
  synergiesAndCounter: SynergiesAndCounter;
  conditionsForGeneralTier: TierRecommendations;
  conditionsForMasterTier: TierRecommendations;
  conditionsForAramTier: TierRecommendations;
};

export interface HeroMapPreference {
  strongerMaps: string[];
  averageMaps: string[];
  weakerMaps: string[];
  strategy: string;
}

export interface SynergiesAndCounter {
  synergicHeroes: string[];
  synergySource: string;
  counteringHeroes: string[];
  counterSource: string;
};

export type TierCondition = "default" | "taunt" | "colossus" | "fury"

export type TierRecommendations = {
  [condition in TierCondition]?: TierRecommendation;
}

export type Tier = "strong-tiers"
  | "good-tier-1" 
  | "decent-tier-2" 
  | "mediocre-tier-3"
  | "unavailable-tier-4"

export type TierList = 'general' | 'master' | 'aram'

export interface TierRecommendation {
  tier: Tier;
  banRecommended: boolean;
}