# Frontend notes:

- Use mantine components and their attributes as much as possible instead of custom CSS
- When you have to use custom CSS, use a proper design system with CSS variables that are re-used
- You should never define new CSS variables that are just renaming existing root CSS variables; simply use the root ones directly
- Use mobx as a state management. Avoid prop drilling, and even passing props altogether if you can just use the store
- Re-use existing components, staying as DRY as possible
- Use your own browser tool to ensure things actually work after big changes. Do not do this for every little thing

# Backend notes:

- Use a routes/controller/service setup
- Each service should have its own directory, and within that directory you can feel free to break it down into subdirectories
- For this project, use ASP.NET Core Web API controllers and services instead of Express.

# General notes

- Keep things simple
- Avoid unnecessary abstractions
- Use consts: no magic numbers or strings allowed
- Helpers/utils should live in helper/util files, and should be re-used when possible
- All files should be 320 lines or fewer, unless there is a very good reason otherwise
- Splitting files/services into directories is a helpful way to keep them from getting too large
- Separate concerns religiously; each file should have a clear purpose
- When reviewing code, ask: “can this be done more simply?”
- Generally, core logic should be easy to follow, with helpers doing the heavy lifting
- Ensure there are no lint or type errors before claiming something is done
