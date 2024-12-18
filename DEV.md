# Commands

## Migrations
~~~bash
# install or update tools
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

# create migration
dotnet ef migrations add Initial --project App.DAL.EF --startup-project WebApp --context AppDbContext

# apply migration
dotnet ef database update --project App.DAL.EF --startup-project WebApp --context AppDbContext
~~~

## Docker
~~~bash
# --build updates the image
docker-compose up --build

# Docker Hub
docker build -t chat-app .
docker tag chat-app romatariq/chat-app:latest
docker push romatariq/chat-app:latest
~~~


# TODOs

- [X] Username displayed as case sensitive but unique as not case sensitive (e.g. Admin == admin)
- [X] Add filters to username ([a-z], [A-Z], [0-9], _, -)
- [X] Remove hard coded admin info (id, password)
- [X] Add paging (verificationController getAll)
- [X] Remove verify user controller, no reason for it
- [X] Add hard coded admin id for dev (so cookies wouldn't display userId that doesn't exist)
- [X] Websocket live messages
- [X] Comment replays - comment can have many replies, each reply can have several replies. But all replies are on single level, they don't create a tree. Meaning reply has parentCommentId and replyToCommentId (which can be null if matches parentCommentId)
- [X] Reports page: show simple report form and stats (report count 24hrs, last hour, can make a graph in future)
- [X] global exception middleware

- [ ] Fix all TODOs

## Functionalities
- [ ] In-app notifications (if someone has replied to you or liked post)

### Nice to have
- [ ] User groups CRUD (group chats and domain/email group, how to create them and invite people)
- [ ] Email confirmation on signup (mvc?, not just link but also has to click a button)
- [ ] External (google) login, maybe even remove own register to avoid implementing email confirmation etc.

### In far future
- [ ] Limit requests
- [ ] Paid user (higher request limit, premium emotes?, etc)
- [ ] Emotes
- [ ] Community emotes
- [ ] Regular website client
- [ ] Text profanity filters
- [ ] Moderator role