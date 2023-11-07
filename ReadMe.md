# QuickStart (API)
- Make sure you have Docker installed
- Run compose-full.sh using a linux kernel
# Detailed help (API + DB)

## Step 1: .env
Make sure to have a .env file with at least the following fields:
- ``URL:[value]``
- ``MYSQL_USER:[value]``
- ``MYSQL_PASSWORD:[value]``
- ``MYSQL_DATABASE:[value]``
- ``MYSQL_ROOT_PASSWORD:[value]``

Make sure to remove ``[value]`` from the fields and add actual data.

## Step 2: The commands
- ``docker compose build .`` - This builds the application.
- ``docker compose up -d`` - This starts the application in detached mode.

## Step 3: Stopping the application

- ``docker compose down``

# Updating the container while it is running

For this you can run the ``updateFiles.sh`` file. You can also make this the executable file for IntelliJ.

- Click on the field to the left of the ``run`` button.
- Click on ``Edit Configurations...``.
- Click on the ``+`` icon on the top left of the window.
- Then choose for ``Shell Script``.
- Here you can choose for a file to be run.
- Choose for ``updateFiles.sh``.

Note: The opened window might not close on its own.