import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Catalog } from './components/Catalog';
import { Inventory } from './components/Inventory';
import { ApplicationPaths } from './components/Constants';
import { Login } from './components/Login';

import './App.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path={ApplicationPaths.CatalogPath} component={Catalog} />
        <Route path={ApplicationPaths.InventoryPath} component={Inventory} />
        <Route path={ApplicationPaths.LoginPath} component={Login} />
      </Layout>
    );
  }
}
