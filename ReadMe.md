# Required

## .env
- Make sure to have a ``.env`` file that mimics the ``.env-config`` file. Add data where necessary.
- Ask Owner what data should be in here if you have no idea.

## Docker
- Make sure you have Docker installed

## .NET 7
- Make sure you are running the application using the ``.NET7`` instance.
- The docker container should take care of this, but if you want to run it locally you will need it.

# QuickStart

- Run ``compose-full.sh`` using a linux kernel

Note: Database might be empty.
# Detailed help

## Step 1: The commands
- ``docker compose build .`` - This builds the application.
- ``docker compose up -d`` - This starts the application in detached mode.

## Step 2: Stopping the application

- ``docker compose down`` - This stops the application.

# Updating the container while it is running (Deprecated?)

For this you can run the ``updateFiles.sh`` file. You can also make this the executable file for IntelliJ.

- Click on the field to the left of the ``run`` button.
- Click on ``Edit Configurations...``.
- Click on the ``+`` icon on the top left of the window.
- Then choose for ``Shell Script``.
- Here you can choose for a file to be run.
- Choose for ``updateFiles.sh``.

Note: The opened window might not close on its own.