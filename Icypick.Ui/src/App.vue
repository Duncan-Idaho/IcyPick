<template>
  <router-view/>
</template>

<style lang="scss">
body {
  background-color: #1a0a38;
}

html, body, #app {
  height: 100%;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: rgba(255, 255, 255, 0.8); 
}

#nav {
  padding: 30px;

  a {
    color: rgba(255, 255, 255, 0.8); 

    &.router-link-exact-active {
      color: rgba(255, 255, 255, 100); 
      font-weight: bold;
    }
  }
}


button {
  font-size: 1.33rem;
  color: white;
  font-weight: bold;

  margin: 0.2rem;
  border: 2px solid rgba(255, 255, 255, 0.7);

  height: 3rem;
  & span.mdi {
    width: 3rem;
  }
  
  // button reset
  background-color: transparent;
  box-sizing: content-box;
  outline: none;
  padding: 0;

  &:focus {
    border: 2px solid rgba(138, 150, 255, 1);
  }
}
  
</style>
