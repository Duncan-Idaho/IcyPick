
import { Hero } from '@/data'

export interface EnrichedHero extends Hero {
  extra: ExtraSynergiesAndCounter;
}

export interface ExtraSynergiesAndCounter {
  synergizeWithHeroes: string[];
  countersHeroes: string[];
}

export function toEnrichedHero(hero: Hero, otherHeroes: Hero[]): EnrichedHero {
  return {
    ...hero,
    extra: {
      synergizeWithHeroes: getSynergizeWithHeroes(hero, otherHeroes),
      countersHeroes: getCountersHeroes(hero, otherHeroes)
    }
  }
}

export function getSynergizeWithHeroes(hero: Hero, otherHeroes: Hero[]) {
  return otherHeroes
    .filter(otherHero => !hero.synergiesAndCounter.synergicHeroes.includes(otherHero.id))
    .filter(otherHero => otherHero.synergiesAndCounter.synergicHeroes.includes(hero.id))
    .map(hero => hero.id)
}

export function getCountersHeroes(hero: Hero, otherHeroes: Hero[]) {
  return otherHeroes
    .filter(otherHero => otherHero.synergiesAndCounter.counteringHeroes.includes(hero.id))
    .map(hero => hero.id)
}
