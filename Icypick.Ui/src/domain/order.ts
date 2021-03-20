
export enum GenericSlotId {
  Map = 'map', 
  Pick = 'pick'
}

interface Indexes {
  ally: number;
  ennemy: number;
  allyBan: number;
  ennemyBan: number;
}

export type SlotKind = keyof Indexes
export interface HeroSlotId { kind: SlotKind; index: number }

export type SlotId = GenericSlotId | HeroSlotId
export type FirstPick = 'ally' | 'ennemy' | undefined

export function getIndexFor(slotId: SlotId | null, kind: SlotKind) {
  if (!slotId)
    return undefined;
  if (typeof slotId === 'string' || slotId.kind !== kind)
    return undefined;
  return slotId.index;
}

export function isHeroSlot(slotId: SlotId | null): slotId is HeroSlotId {
  return !!slotId && typeof slotId !== 'string';
}

const order = 'm 1 bB bB p PP pp B b PP pp P'


function createSlot(orderCode: string, indexes: Indexes, firstPick: FirstPick): SlotId | undefined {
  if (orderCode === 'm')
    return GenericSlotId.Map
  if (orderCode === '1')
    return GenericSlotId.Pick
  if (!firstPick || !['p', 'b'].includes(orderCode.toLowerCase()))
    return undefined;

  const isFirstPickAlly = firstPick === 'ally'
  const isOrderFirstPick = orderCode.toLowerCase() === orderCode
  const team = isOrderFirstPick === isFirstPickAlly ? 'ally': 'ennemy'
  const kind = orderCode.toLowerCase() === 'p' ? team : team + 'Ban' as SlotKind
  return { 
    kind, 
    index: indexes[kind]++ 
  }
}

export function createSlots(firstPick: FirstPick): SlotId[] {
  const result = []
  const indexes = {
    'ally': 0,
    'ennemy': 0,
    'allyBan': 0,
    'ennemyBan': 0,
  }

  for (const orderCode of order) {
    const slot = createSlot(orderCode, indexes, firstPick)
    if (slot)
      result.push(slot)
  }

  return result
}
