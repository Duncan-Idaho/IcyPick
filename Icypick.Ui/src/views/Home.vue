<template>
  <div class="layout">
    <div class="header">
      <MapSlot :map="selectedMap" bar @click="unselectMap"/>
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
        <HeroSelector v-else v-model="selectedHero"/>
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
interface HeroSlotId { kind: 'ally' | 'ennemy'; index: number }

type SlotId = GenericSlotId | HeroSlotId

interface Data {
  selectedMap: Map | null;
  allies: (Hero | undefined)[];
  ennemies: (Hero | undefined)[];
  selectedSlot: SlotId;
}

function getIndexFor(slotId: SlotId, kind: 'ally' | 'ennemy') {
  if (typeof slotId === 'string' || slotId.kind !== kind)
    return undefined;
  return slotId.index;
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
    selectedHero: {
      get(): Hero | undefined {
        if (typeof this.selectedSlot === 'string')
          return undefined

        const slots: (Hero | undefined)[] = this.selectedSlot.kind === 'ally'
          ? this.allies
          : this.ennemies
          
        return slots[this.selectedSlot.index]
      },
      set(value: Hero |  undefined) {
        if (typeof this.selectedSlot === 'string')
          return

        const slots: (Hero | undefined)[] = this.selectedSlot.kind === 'ally'
          ? this.allies
          : this.ennemies
          
        slots[this.selectedSlot.index] = value
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
  justify-content: center;
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