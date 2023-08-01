import os
import subprocess
from datetime import datetime, timedelta

# Function to create a commit with a specific message and date
def create_commit(commit_message, commit_date):
    # Set the environment variables for the commit date
    os.environ['GIT_AUTHOR_DATE'] = commit_date
    os.environ['GIT_COMMITTER_DATE'] = commit_date
    
    # Add all changes to the staging area
    subprocess.run(['git', 'add', '.'])
    
    # Commit with the specified message
    subprocess.run(['git', 'commit', '-m', commit_message])

# Example usage: Creating commits in the past
base_date = datetime(2023, 8, 1)  # Starting date for commits
number_of_commits = 5

for i in range(number_of_commits):
    # Calculate the date for each commit
    commit_date = (base_date + timedelta(days=i)).strftime('%Y-%m-%dT%H:%M:%S')
    commit_message = f'Commit number {i+1} on {commit_date}'
    create_commit(commit_message, commit_date)

# Push the commits to GitHub
subprocess.run(['git', 'push', 'origin', 'main'])
