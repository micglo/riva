# Riva

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 10.0.5.

## Development server

1. Run `npm install` to install the dependencies.
2. Run `build:lib` to build all libraries.
3. Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Applications

There are a few applications:

| Application                                               | Serve command                            | Local url                                        |
|:---------------------------------------------------------:|:----------------------------------------:|:------------------------------------------------:|
| Riva Portal (`riva-portal`) - default                     | `ng serve riva-portal`                   | [http://localhost:4200/](http://localhost:4200/) |
| Riva Administrative Portal (`riva-administrative-portal`) | `ng serve riva-administrative-portal`    | [http://localhost:42001](http://localhost:4201/) |

## Build

Run `ng build` to build the default project. The build artifacts will be stored in the `dist/riva-portal/` directory.
To run `ng` command for a single project please use the name of the project as an argument. For example: `ng build riva-portal` to build Farm Portal application.
To build a project with `production` configuration run:
`npm run build:rivaportal` - for `riva-portal`
`npm run build:rivaadministrativeportal` - for `riva-administrative-portal`

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Other useful commands

Run `ng lint` to run applications linting.
Run `npm run lint:fix` to run correct all lint issues.
Run `npm run prettier:check` to check issues with code formatting.
Run `npm run prettier:fix` to fix all issues with code formatting.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
