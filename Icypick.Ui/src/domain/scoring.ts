
import { Map, Hero, TierList, TierRecommendations, TierCondition } from '@/data'
import { EnrichedHero } from './enriching'

const mapWeight = 3
const synergyWeight = 1
const counteredWeight = -2
const counteringWeight = 2
const tierScores = {
  "strong-tiers": { score: 5, reason: 'Strong tier' },
  "good-tier-1": { score: 3, reason: 'Good tier' },
  "decent-tier-2": { score: 0, reason: 'Decent tier' },
  "mediocre-tier-3": { score: -3, reason: 'Mediocre tier' },
  "unavailable-tier-4": { score: -100, reason: 'Unavailable' },
}

export interface ScoringContext {
  selectedMap: Map | null;
  allies: (Hero | undefined)[];
  ennemies: (Hero | undefined)[];
  allyBans: (Hero | undefined)[];
  ennemyBans: (Hero | undefined)[];
}

export interface ScoredHero extends EnrichedHero {
  score: number;
  scoreReasons: ScoreReason[];
}

export interface ScoreReason {
  score: number;
  reason: string;
  inactive?: boolean;
}

export function toScoredHero(hero: EnrichedHero): ScoredHero {
  return {
      ...hero,
    score: 0,
    scoreReasons: []
  };
}

export function scoreHero(hero: ScoredHero, context: ScoringContext) {
  hero.scoreReasons = [
    ...scoreMap(hero, context),
    ...scoreSynergies(hero, context),
    ...scoreCountered(hero, context),
    ...scoreCountering(hero, context),
    ...scoreTiers(hero, context)
  ].filter(defined);

  hero.score = hero.scoreReasons
    .filter(score => !score.inactive)
    .reduce((total, score) => total + score.score, 0)

  return hero
}

function scoreMap(hero: ScoredHero, context: ScoringContext) {
  if (!context.selectedMap) 
    return []

  if (hero.mapPreference.strongerMaps.includes(context.selectedMap.id))
    return [{ score: mapWeight, reason: 'Strong on map ' + context.selectedMap.name }]

  if (hero.mapPreference.weakerMaps.includes(context.selectedMap.id))
    return [{ score: -mapWeight, reason: 'Weak on map ' + context.selectedMap.name }]

  return []
}

function scoreSynergies(hero: ScoredHero, context: ScoringContext) {
  return scoreBasedOnOtherHeroes(
    context.allies, 
    hero.synergiesAndCounter.synergicHeroes.concat(hero.extra.synergizeWithHeroes), 
    synergyWeight, 
    'Synergize with ')
}

function scoreCountered(hero: ScoredHero, context: ScoringContext) {
  return scoreBasedOnOtherHeroes(
    context.ennemies, 
    hero.synergiesAndCounter.counteringHeroes, 
    counteredWeight, 
    'Countered by ')
}

function scoreCountering(hero: ScoredHero, context: ScoringContext) {
  return scoreBasedOnOtherHeroes(
    context.ennemies, 
    hero.extra.countersHeroes, 
    counteringWeight, 
    'Counters ')
}

function scoreBasedOnOtherHeroes(others: (Hero | undefined)[], potentialIds: string[], score: number, reasonPrefix: string) {
  return potentialIds
    .map(potentialId => others.find(other => other && other.id === potentialId ))
    .filter(defined)
    .map(hero => ({ score, reason: reasonPrefix + hero.name }))
}

function scoreTiers(hero: ScoredHero, context: ScoringContext) {
  const tierList = 'general';
  const tierRecommendations = hero[getTierListProperty(tierList)] as TierRecommendations;
  const scores: ScoreReason[] = [
    getScoreForTier(tierRecommendations, 'default'),
    getScoreForTier(tierRecommendations, 'colossus'),
    getScoreForTier(tierRecommendations, 'fury'),
    getScoreForTier(tierRecommendations, 'taunt'),
  ].filter(defined)

  if (scores.length === 0)
    return scores

  const maxScore = scores.reduce((max, current) => current.score > max.score ? current : max)
  scores.filter(score => score !== maxScore)
    .forEach(score => score.inactive = true)

  return scores
}

function getScoreForTier(recommendations: TierRecommendations, condition: TierCondition) {
  const recommendation = recommendations[condition]

  if (recommendation) {
    const score = tierScores[recommendation.tier]
    if (condition === 'default') 
      return score
    
    return {
      score: score.score,
      reason: `${score.reason} (${condition})`
    }
  }
}

function getTierListProperty(tierList: TierList): keyof Hero {
  switch (tierList) {
    case 'general': 
      return 'conditionsForGeneralTier';
    case 'master': 
      return 'conditionsForMasterTier';
    case 'aram': 
      return 'conditionsForAramTier';
  }
}

function defined<TValue>(value: TValue | undefined): value is TValue {
  return value !== undefined
}