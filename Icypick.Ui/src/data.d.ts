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
  category: string;
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

export interface TierRecommendations {
  [condition: "default" | "taunt" | "colossus" | "fury"]: TierRecommendation;
}

export interface TierRecommendation {
  tier: string;
  banRecommended: boolean;
}