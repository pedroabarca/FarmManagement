# Railway Deployment Guide - FarmManagement API

## Quick Deploy Steps

### 1. Create New Railway Project

1. Go to: https://railway.app/
2. Click "Start a New Project"
3. Select "Deploy from GitHub repo"
4. Connect your GitHub account and select this repository
5. Select the `FarmManagement` directory

### 2. Configure Environment Variables

Add these variables in Railway dashboard (Settings → Variables):

```
DATABASE_PROVIDER=PostgreSQL
ConnectionStrings__DefaultConnection=Host=farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com;Database=farmdb;Username=postgres;Password=123?Ab4rkAl0nso.;Port=5432;SSL Mode=Require;Trust Server Certificate=true
ASPNETCORE_ENVIRONMENT=Production
PORT=8080
```

### 3. Configure Build Settings

Railway should auto-detect the Dockerfile. If not:

- **Root Directory**: `/FarmManagement`
- **Dockerfile Path**: `Dockerfile`
- **Port**: `8080`

### 4. Deploy

Click "Deploy" and wait for the build to complete (~3-5 minutes).

### 5. Get Your URL

Once deployed, Railway will give you a public URL like:
```
https://your-app-name.up.railway.app
```

Test it:
```bash
curl https://your-app-name.up.railway.app/api/animals
```

## Alternative: CLI Deployment

If you prefer CLI:

```bash
cd /Users/pedroabarca/Projects/FARM/FarmManagement

# Login to Railway (opens browser)
railway login

# Initialize project
railway init

# Add environment variables
railway variables set DATABASE_PROVIDER=PostgreSQL
railway variables set "ConnectionStrings__DefaultConnection=Host=farmdb.cd2m8ewim596.us-east-2.rds.amazonaws.com;Database=farmdb;Username=postgres;Password=123?Ab4rkAl0nso.;Port=5432;SSL Mode=Require;Trust Server Certificate=true"
railway variables set ASPNETCORE_ENVIRONMENT=Production
railway variables set PORT=8080

# Deploy
railway up
```

## Troubleshooting

**Build fails?**
- Check Dockerfile is in the root of FarmManagement directory
- Verify all .csproj files are accessible

**Database connection fails?**
- Verify AWS RDS security group allows Railway's IPs
- Test connection string manually

**API doesn't respond?**
- Check Railway logs: `railway logs`
- Verify PORT environment variable is set to 8080
