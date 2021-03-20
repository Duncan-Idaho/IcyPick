<template>
  <div class="layout">
    <div class="header">
      <div class="ally-bans-area">
        <HeroJaggedRows 
          :heroes="allyBans" 
          :row-size="3"
          :modelValue="selectedAllyBanSlot"
          @update:modelValue="onAllyBanSlotClicked"
          />
      </div>
      <div class="map-area">
        <MapSlot :map="selectedMap" bar @click="unselectMap"/>
      </div>
      <div class="ennemy-bans-area">
        <HeroJaggedRows 
          :heroes="ennemyBans" 
          :row-size="3"
          :modelValue="selectedEnnemyBanSlot"
          @update:modelValue="onEnnemyBanSlotClicked"
          /></div>
    </div>
    <div class="body">
      <div class="allies-area">
        <HeroJaggedRows 
          :heroes="allies" 
          :row-size="1"
          alignment="left"
          :modelValue="selectedAllySlot"
          @update:modelValue="onAllySlotClicked"
          />
      </div>
      <div class="main-area">
        <MapSelector v-if="selectedSlot === 'map'" v-model="selectedMap"/>
        <HeroSelector v-if="!!selectedSlot && !!selectedSlot.kind" v-model="selectedHero"/>
      </div>
      <div class="ennemies-area">
        <HeroJaggedRows 
          :heroes="ennemies" 
          :row-size="1"
          alignment="right"
          :modelValue="selectedEnnemySlot"
          @update:modelValue="onEnnemySlotClicked"
          />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
import MapSelector from '@/components/MapSelector.vue';
import MapSlot from '@/components/MapSlot.vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue';
import { Map, Hero } from '@/data';

enum GenericSlotId {
  Map = 'map', 
  Pick = 'pick'
}

type SlotKind = 'ally' | 'ennemy' | 'allyBan' | 'ennemyBan'
interface HeroSlotId { kind: SlotKind; index: number }

type SlotId = GenericSlotId | HeroSlotId

interface Data {
  selectedMap: Map | null;
  allies: (Hero | undefined)[];
  ennemies: (Hero | undefined)[];
  allyBans: (Hero | undefined)[];
  ennemyBans: (Hero | undefined)[];
  selectedSlot: SlotId | null;
}

function getIndexFor(slotId: SlotId | null, kind: SlotKind) {
  if (!slotId)
    return undefined;
  if (typeof slotId === 'string' || slotId.kind !== kind)
    return undefined;
  return slotId.index;
}

function isHeroSlot(slotId: SlotId | null): slotId is HeroSlotId {
  return !!slotId && typeof slotId !== 'string';
}

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
    MapSlot,
    MapSelector,
    HeroJaggedRows
  },
  data(): Data {
    return {
      selectedMap: null,
      allies: Array(Math.ceil(5)).fill(undefined),
      ennemies: Array(Math.ceil(5)).fill(undefined),
      allyBans: Array(Math.ceil(3)).fill(undefined),
      ennemyBans: Array(Math.ceil(3)).fill(undefined),
      selectedSlot: GenericSlotId.Map
    }
  },
  computed: {
    selectedAllySlot(): number | undefined {
      return getIndexFor(this.selectedSlot, 'ally')
    },
    selectedEnnemySlot(): number | undefined {
      return getIndexFor(this.selectedSlot, 'ennemy')
    },
    selectedAllyBanSlot(): number | undefined {
      return getIndexFor(this.selectedSlot, 'allyBan')
    },
    selectedEnnemyBanSlot(): number | undefined {
      return getIndexFor(this.selectedSlot, 'ennemyBan')
    },
    selectedHeroRow(): (Hero | undefined)[] | undefined {
      if (!this.selectedSlot || typeof this.selectedSlot === 'string')
        return undefined

      switch (this.selectedSlot.kind)
      {
        case 'ally':
          return this.allies
        case 'ennemy':
          return this.ennemies
        case 'allyBan':
          return this.allyBans
        case 'ennemyBan':
          return this.ennemyBans
        default:
          return undefined
      }
    },
    selectedHero: {
      get(): Hero | undefined {
        if (!this.selectedHeroRow || !isHeroSlot(this.selectedSlot))
          return undefined;
        return this.selectedHeroRow[this.selectedSlot.index]
      },
      set(value: Hero |  undefined) {
        if (!this.selectedHeroRow || !isHeroSlot(this.selectedSlot))
          return;

        this.selectedHeroRow[this.selectedSlot.index] = value
      }
    }
  },
  methods: {
    unselectMap() {
      this.selectedMap = null
    },
    selectSlot(slotId: SlotId) {
      this.selectedSlot = slotId
    },
    onAllySlotClicked( index: number ) {
      this.selectedSlot = { kind: 'ally', index }
    },
    onEnnemySlotClicked( index: number ) {
      this.selectedSlot = { kind: 'ennemy', index }
    },
    onAllyBanSlotClicked( index: number ) {
      this.selectedSlot = { kind: 'allyBan', index }
    },
    onEnnemyBanSlotClicked( index: number ) {
      this.selectedSlot = { kind: 'ennemyBan', index }
    },
    selectNextSlot() {
      if (!this.selectedMap) {
        this.selectedSlot = GenericSlotId.Map
      } else {
        let firstAllyNotSelected = this.allies.indexOf(undefined);
        let firstEnnemyNotSelected = this.ennemies.indexOf(undefined);

        if (firstAllyNotSelected === -1)
          firstAllyNotSelected = 5;
        if (firstEnnemyNotSelected === -1)
          firstEnnemyNotSelected = 5;

        this.selectedSlot = { 
          kind: firstAllyNotSelected <= firstEnnemyNotSelected ? 'ally' : 'ennemy',
          index: Math.min(firstAllyNotSelected, firstEnnemyNotSelected)
        };

        if (this.selectedSlot.index ===5)
          this.selectedSlot = null
      }
    }
  },
  watch: {
    allies: {
      deep: true,
      handler () {
        this.selectNextSlot()
      }
    },
    ennemies: {
      deep: true,
      handler () {
        this.selectNextSlot()
      }
    },
    allyBans: {
      deep: true,
      handler () {
        this.selectNextSlot()
      }
    },
    ennemyBans: {
      deep: true,
      handler () {
        this.selectNextSlot()
      }
    },
    selectedMap: {
      deep: true,
      handler () {
        this.selectNextSlot()
      }
    }
  }
});
</script>

<style lang="scss" scoped>*
.layout {
  width: 100%;
  height: 100%;
  
  display: flex;
  flex-flow: column nowrap;
}

.header {
  display: flex;
  justify-content: space-around;
}

.body {
  flex: 1;
  overflow: hidden;
  
  display: flex;
  flex-flow: row nowrap;
}

.allies-area, .ennemies-area {
  --hero-width: 10rem;
}

.allies-area {
  margin-right: 0.5em;
}

.ennemies-area {
  margin-left: 0.5em;
}

.main-area {
  flex: 1;
  overflow: auto;

  display: flex;
  flex-flow: column nowrap;
  align-items: center;  
}
</style>